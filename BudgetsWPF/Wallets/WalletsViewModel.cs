using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Budgets.BusinessLayer.Entities;
using Budgets.Common;
using BudgetsStorage.Services;
using BudgetsWPF.Navigation;
using Prism.Commands;
using Prism.Mvvm;

namespace BudgetsWPF.Wallets
{
    class WalletsViewModel: BindableBase, INavigatable
    {
        private WalletDetailsViewModel _currentWallet;

        public ObservableCollection<WalletDetailsViewModel> Wallets { get; set; }
        private User _user;

        public DelegateCommand SignInCommand { get; }
        public DelegateCommand AddWalletCommand { get; }
        public DelegateCommand GoToCategoriesCommand { get; }
        public DelegateCommand SaveUserInfoCommand { get; }
        public DelegateCommand ShareWalletsCommand { get; }

        public WalletsViewModel( User user )
        {
            _user = user;

            Wallets = new ObservableCollection<WalletDetailsViewModel>();
            SignInCommand = new DelegateCommand(() => MainNavigator.Navigate(NavigatableType.SignIn));
            AddWalletCommand = new DelegateCommand(AddWallet);
            
            SaveUserInfoCommand = new DelegateCommand(SaveUserInfo, CanSaveUserInfo);
            ShareWalletsCommand = new DelegateCommand(ShareWallets);
            GoToCategoriesCommand = new DelegateCommand(GoToCategories);

            foreach (var wallet in _user.Wallets)
            {
                Wallets.Add(new WalletDetailsViewModel(user, wallet, RemoveWallet));
            }
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

        public async void ShareWallets()
        {
            IsEnabled = false;
            var musers = await UserService.All;
            var users = musers.Values.Where(u => !u.Equals(_user)).ToList();
            MainNavigator.Navigate(NavigatableType.Share, _user, users);
            IsEnabled = true;
        }

        public async void SaveUserInfo()
        {
            IsEnabled = false;
            await UserService.Add(_user);
            _user.HasChanges = false;
            SaveUserInfoCommand.RaiseCanExecuteChanged();
            IsEnabled = true;
        }
        public bool CanSaveUserInfo() { return _user.HasChanges && _user.IsValid; }

        public async void GoToCategories()
        {
            IsEnabled = false;
            if (_user.Categories.Count == 0) 
                await CategoryService.FillCategories(_user);
            MainNavigator.Navigate(NavigatableType.Categories, _user);
            IsEnabled = true;
        }

        public void RemoveWallet(WalletDetailsViewModel wd)
        {
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
                SaveUserInfoCommand.RaiseCanExecuteChanged();
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
                SaveUserInfoCommand.RaiseCanExecuteChanged();
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

        public NavigatableType Type => NavigatableType.Wallets;

        public void AddWallet()
        {
            Wallet w = new Wallet(_user.Id, "", "", 0, Currency.UAH);
            _user.AddWallet(w);
            Wallets.Add(new WalletDetailsViewModel(_user, w, RemoveWallet));
            RaisePropertyChanged(nameof(Wallets));
        }


    }
}
