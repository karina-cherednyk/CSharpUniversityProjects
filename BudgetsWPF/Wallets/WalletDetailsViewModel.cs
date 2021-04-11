using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Budgets.BusinessLayer.Entities;
using Budgets.Common;
using BudgetsStorage.Services;
using Prism.Commands;
using Prism.Mvvm;

namespace BudgetsWPF.Wallets
{
    class WalletDetailsViewModel: BindableBase
    {
        public ObservableCollection<Category> Categories;
        private string _selectedCurrency;
        private Action<WalletDetailsViewModel> _removeWalletFromWalletsView;
        private User _user;
        public Wallet _wallet;

        public DelegateCommand RemoveWalletCommand { get; }
        public DelegateCommand ShowTransactionsCommand { get; }
        public DelegateCommand SaveWalletCommand { get;  }

   

        public WalletDetailsViewModel(User user,  Wallet wallet, Action<WalletDetailsViewModel> removeWallet, Action<User, Wallet> goToTransactions)
        {
            _user = user;
            _wallet = wallet;
            Categories = new(wallet.Categories);
            _selectedCurrency = CurrencyConvertor.CurencyToString(_wallet.Currency);
            _removeWalletFromWalletsView = removeWallet;
            ShowTransactionsCommand = new DelegateCommand(() => goToTransactions(user, _wallet));
            SaveWalletCommand = new DelegateCommand(SaveWallet, CanSaveWallet);
            RemoveWalletCommand = new DelegateCommand(RemoveWallet, CanRemoveWallet);
        }

        public bool CanSaveWallet() => _wallet.HasChanges && _wallet.IsValid;

        public bool CanRemoveWallet() => !_wallet.IsNew;

        public async void RemoveWallet()
        {
            await RelationService<User, Wallet>.RemoveConnection(_user, _wallet);
            _user.RemoveWallet(_wallet);
            _removeWalletFromWalletsView(this);
        }
        public async void SaveWallet()
        {
            await WalletService.Add(_wallet);
            await RelationService<User, Wallet>.AddConnection(_user, _wallet);

            _wallet.HasChanges = false;
            _wallet.IsNew = false;
            RemoveWalletCommand.RaiseCanExecuteChanged();
            SaveWalletCommand.RaiseCanExecuteChanged();
            RaisePropertyChanged(nameof(DisplayName));
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
                _wallet.Currency = CurrencyConvertor.CurrencyFromString(value);

                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Balance));
                RaisePropertyChanged(nameof(MonthLoss));
                RaisePropertyChanged(nameof(MonthProfit));
                RaisePropertyChanged(nameof(DisplayName));
            }
        }
        public List<string> Currencies
        {
            get
            {
                return CurrencyConvertor.AllCurrencies();
            }
        }

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
                }
                RaisePropertyChanged(nameof(DisplayName));
            }
        }

        public string Balance => _wallet.Balance.ToString("F");
        public string MonthLoss => _wallet.MonthLoss.ToString("F");
        public string MonthProfit => _wallet.MonthProfit.ToString("F");

    }
}
