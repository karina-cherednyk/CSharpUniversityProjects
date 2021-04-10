using System;
using System.Collections.ObjectModel;
using Budgets.BusinessLayer.Entities;
using Budgets.Common;
using BudgetsStorage.Services;
using Prism.Commands;
using Prism.Mvvm;

namespace BudgetsWPF.Wallets
{
    class WalletsViewModel: BindableBase
    {
        private WalletDetailsViewModel _currentWallet;
        private readonly Action _goToSignIn;
        private readonly Action<User,Wallet> _goToTransactions;
        private readonly Action<User> _goToCategories;

        public ObservableCollection<WalletDetailsViewModel> Wallets { get; set; }
        private User _user;

        public DelegateCommand SignInCommand { get; }
        public DelegateCommand AddWalletCommand { get; }
        public DelegateCommand GoToCategoriesCommand { get;  }


        public WalletsViewModel( User user, Action goToSignIn, Action<User, Wallet> goToTransactions, Action <User> goToCategories)
        {
            _user = user;
            _goToSignIn = goToSignIn;
            _goToTransactions = goToTransactions;
            _goToCategories = goToCategories;

            Wallets = new ObservableCollection<WalletDetailsViewModel>();
            SignInCommand = new DelegateCommand(_goToSignIn);
            AddWalletCommand = new DelegateCommand(AddWallet);
            
            GoToCategoriesCommand = new DelegateCommand(GoToCategories);

            foreach (var wallet in _user.Wallets)
            {
                Wallets.Add(new WalletDetailsViewModel(user, wallet, RemoveWallet, _goToTransactions));
            }
        }
        public async void GoToCategories()
        {
            if(_user.Categories.Count == 0) 
                await CategoryService.FillCategories(_user);
            _goToCategories(_user);
        }

        public void RemoveWallet(WalletDetailsViewModel wd)
        {
            _user.RemoveWallet(wd.Wallet);
            Wallets.Remove(wd);
            RaisePropertyChanged(nameof(CurrentWallet));
            RaisePropertyChanged(nameof(Wallets));
        }

        public string Name
        {
            get
            {
                return _user.FirstName;
            }
            set
            {
                _user.FirstName = value;
                RaisePropertyChanged();
            }
        }
        
        public string Surname
        {
            get
            {
                return _user.LastName;
            }
            set
            {
                _user.LastName = value;
                RaisePropertyChanged();
            }
        }


        public WalletDetailsViewModel CurrentWallet
        {
            get
            {
                return _currentWallet;
            }
            set
            {
                _currentWallet = value;
                RaisePropertyChanged();
            }
        }

        public void AddWallet()
        {
            Wallet w = new Wallet(_user.Id, "New wallet", "Some wallet", 0, Currency.UAH);
            _user.AddWallet(w);
            Wallets.Add(new WalletDetailsViewModel(_user, w, RemoveWallet, _goToTransactions));
            RaisePropertyChanged(nameof(Wallets));
        }


    }
}
