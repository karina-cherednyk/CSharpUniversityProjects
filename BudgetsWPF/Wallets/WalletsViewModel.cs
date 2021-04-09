using System;
using System.Collections.ObjectModel;
using Budgets.BusinessLayer.Entities;
using Budgets.Common;
using Prism.Commands;
using Prism.Mvvm;

namespace BudgetsWPF.Wallets
{
    class WalletsViewModel: BindableBase
    {
        private WalletDetailsViewModel _currentWallet;
        private readonly Action _goToSignIn;

        public ObservableCollection<WalletDetailsViewModel> Wallets { get; set; }
        private User _user;

        public DelegateCommand SignInCommand { get; }
        public DelegateCommand AddWalletCommand { get; }


        public WalletsViewModel( User user, Action goToSignIn, Action<User, Wallet> goToTransactions)
        {
            _user = user;
            _goToSignIn = goToSignIn;

            Wallets = new ObservableCollection<WalletDetailsViewModel>();
            SignInCommand = new DelegateCommand(_goToSignIn);
            AddWalletCommand = new DelegateCommand(AddWallet);

            foreach (var wallet in _user.Wallets)
            {
                Wallets.Add(new WalletDetailsViewModel(user, wallet, RemoveWallet, goToTransactions));
            }
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
            Wallets.Add(new WalletDetailsViewModel(w, RemoveWallet));
            RaisePropertyChanged(nameof(Wallets));
        }
    }
}
