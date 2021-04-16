using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Budgets.BusinessLayer.Entities;
using Budgets.Common;
using BudgetsStorage.Services;
using BudgetsWPF.Navigation;
using Prism.Commands;
using Prism.Mvvm;

namespace BudgetsWPF.Wallets
{
    class WalletDetailsViewModel : BindableBase
    {
        private string _selectedCurrency;
        private Action<WalletDetailsViewModel> _removeWalletFromWalletsView;
        private User _user;
        private Wallet _wallet;
        private HashSet<Category> _oldCategories;


        public DelegateCommand RemoveWalletCommand { get; }
        public DelegateCommand ShowTransactionsCommand { get; }
        public DelegateCommand SaveWalletCommand { get; }
        public DelegateCommand ToggleCategoryCommand { get; }

        

        public WalletDetailsViewModel(User user,  Wallet wallet, Action<WalletDetailsViewModel> removeWallet)
        {
            _user = user;
            _wallet = wallet;
            _oldCategories = new();
            _wallet.Categories.ToList().ForEach(c => _oldCategories.Add(c));
            _selectedCurrency = CurrencyConvertor.CurencyToString(_wallet.Currency);
            _removeWalletFromWalletsView = removeWallet;
            ShowTransactionsCommand = new DelegateCommand(ShowTransactions);
            SaveWalletCommand = new DelegateCommand(SaveWallet, CanSaveWallet);
            RemoveWalletCommand = new DelegateCommand(RemoveWallet, CanRemoveWallet);
            ToggleCategoryCommand = new DelegateCommand(ToggleCategory);
            CategoryBtnText = CHOOSE_CAT;

            IsOwner = _user.Id.Equals(wallet.Owner);
        }

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            private set
            {
                _isEnabled = value;
                RaisePropertyChanged();
            }
        }
        public bool IsOwner { get; }

        public bool CanSaveWallet() => _wallet.HasChanges && _wallet.IsValid;

        public bool CanRemoveWallet() => IsOwner &&  !_wallet.IsNew;


        public async void ShowTransactions()
        {
            IsEnabled = false;
            if (_wallet.Categories.Count == 0)
            {
                string err = "Wallet has no categories\nCan`t add transaction\nStay here and add category?";
                MessageBoxResult result = MessageBox.Show(err, "No categories", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No)
                {
                    await TransactionService.FillTransactions(_wallet);
                    MainNavigator.Navigate(NavigatableType.Transactions, _user, _wallet);
                }
            }
            else
            {
                await TransactionService.FillTransactions(_wallet);
                MainNavigator.Navigate(NavigatableType.Transactions, _user, _wallet);
            }
            IsEnabled = true;
        }

        public async void RemoveWallet()
        {
            IsEnabled = false;
            await RelationService<User, Wallet>.RemoveConnection(_user, _wallet);
            _user.RemoveWallet(_wallet);
            _removeWalletFromWalletsView(this);

            IsEnabled = true;
        }
        public async void SaveWallet()
        {
            IsEnabled = false;

            await WalletService.Add(_wallet);
            await RelationService<User, Wallet>.AddConnection(_user, _wallet);


            if (!_oldCategories.SetEquals(_wallet.Categories))
            {
                var minusCats = _oldCategories.Except(_wallet.Categories);
                var plusCats = _wallet.Categories.Except(_oldCategories);
                foreach(var m in minusCats)
                {
                    await RelationService<Wallet, Category>.RemoveConnection(_wallet, m);
                }
                foreach(var p in plusCats)
                {
                    await RelationService<Wallet, Category>.AddConnection(_wallet, p);
                }
            }

            _wallet.HasChanges = false;
            _wallet.IsNew = false;
            RemoveWalletCommand.RaiseCanExecuteChanged();
            SaveWalletCommand.RaiseCanExecuteChanged();
            RaisePropertyChanged(nameof(DisplayName));

            IsEnabled = true;
        }


        public string SelectedCurrency
        {
            get
            {
                return _selectedCurrency;
            }
            set
            {
                _selectedCurrency = value;
                if (IsOwner)
                {
                    _wallet.Currency = CurrencyConvertor.CurrencyFromString(value);
                }
                else
                {
                    bool changes = _wallet.HasChanges;
                    _wallet.Currency = CurrencyConvertor.CurrencyFromString(value);
                    _wallet.HasChanges = changes;
                }


                RaisePropertyChanged();
                RaisePropertyChanged(nameof(InitialBalance));
                RaisePropertyChanged(nameof(Balance));
                RaisePropertyChanged(nameof(MonthLoss));
                RaisePropertyChanged(nameof(MonthProfit));
                RaisePropertyChanged(nameof(DisplayName));
            }
        }

        private const string    SAVE_CAT = "Save Category",
                                REM_CAT = "Remove Category",
                                CHOOSE_CAT = "Choose category";
        public string CategoryBtnText { get; set;  }
        
        public void ToggleCategory()
        {
            if (CategoryBtnText == SAVE_CAT)
            {
                if (SelectedUserCategory == null)
                    return;
                _wallet.AddCategory(SelectedUserCategory);
            }
            else if (CategoryBtnText == REM_CAT && _user.HasCategory(SelectedWalletCategory))
            {
                _wallet.RemoveCategory(SelectedWalletCategory);
            }
            else if (SelectedWalletCategory == null && _user.HasCategory(SelectedWalletCategory)) return;
            else
            {
                CategoryBtnText = CHOOSE_CAT;
                return;
            }

            CategoryBtnText = CHOOSE_CAT;
            SelectedUserCategory = null;
            

            RaisePropertyChanged(nameof(UserCategories));
            RaisePropertyChanged(nameof(SelectedUserCategory));
            RaisePropertyChanged(nameof(SelectedWalletCategory));
            RaisePropertyChanged(nameof(Categories));
            SaveWalletCommand.RaiseCanExecuteChanged();
        }

        private Category _selectedWalletCategory;
        public Category SelectedWalletCategory
        {
            get
            {
                return _selectedWalletCategory;
            }
            set
            {
                _selectedWalletCategory = value;
                if(value != null ) 
                    CategoryBtnText = REM_CAT;
                RaisePropertyChanged(nameof(CategoryBtnText));
            }
        }
        private Category _selectedUserCategory;
        public Category SelectedUserCategory
        {
            get
            {
                return _selectedUserCategory;
            }
            set
            {
                _selectedUserCategory = value;
                if(value != null)
                    CategoryBtnText = SAVE_CAT;
                RaisePropertyChanged(nameof(CategoryBtnText));
            }
        }

        public List<Category> Categories =>  _wallet.Categories.ToList();


        public List<string> Currencies
        {
            get
            {
                return CurrencyConvertor.AllCurrencies();
            }
        }

        public List<Category> UserCategories => _user.Categories.Where(x => !_wallet.Categories.Contains(x)).ToList();


        public string DisplayName
        {
            get
            {
                if (_wallet.IsNew)
                    return "New wallet";

                return $"{_wallet.Name} ({Balance}{CurrencyConvertor.CurencyToSign(_wallet.Currency)})";
            }
        }
        public string Description
        {
            get
            {
                return _wallet.Description;
            }
            set
            {
                _wallet.Description = value;
                SaveWalletCommand.RaiseCanExecuteChanged();
            }
        }

        public string Name
        {
            get
            {
                return _wallet.Name;
            }
            set
            {
                _wallet.Name = value;
                RaisePropertyChanged(nameof(DisplayName));
                SaveWalletCommand.RaiseCanExecuteChanged();
            }
        }
        public string InitialBalance
        {
            get
            {
                return _wallet.InitialBalance.ToString("F");
            }
            set
            {
                RaisePropertyChanged(nameof(DisplayName));
                decimal val;
                if (decimal.TryParse(value, out val))
                {
                    _wallet.InitialBalance = val;
                    SaveWalletCommand.RaiseCanExecuteChanged();
                    RaisePropertyChanged(nameof(DisplayName));
                    RaisePropertyChanged(nameof(Balance));
                }
            }
        }

        public string Balance => _wallet.Balance.ToString("F");
        public string MonthLoss => _wallet.MonthLoss.ToString("F");
        public string MonthProfit => _wallet.MonthProfit.ToString("F");

    }
}
