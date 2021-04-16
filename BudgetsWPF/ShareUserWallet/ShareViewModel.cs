using Budgets.BusinessLayer.Entities;
using BudgetsStorage.Services;
using BudgetsWPF.Navigation;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BudgetsWPF.ShareUserWallet
{
    public class ShareViewModel: BindableBase, INavigatable
    {
        private User _user;
        private List<User> _users;
        private User _selectedUser;
        private Wallet _selectedWallet;
        public DelegateCommand ShareCommand { get; }
        public DelegateCommand ToWalletsCommand { get; }
        public ShareViewModel(User user, List<User> users)
        {
            _user = user;
            _users = users;
            ToWalletsCommand = new DelegateCommand(() => MainNavigator.Navigate(NavigatableType.Wallets, _user));
            ShareCommand = new DelegateCommand(Share, CanShare);

        }

        private bool _isEnabled = true;
        public bool IsEnabled { 
            get { return _isEnabled; } 
            private set {
                _isEnabled = value;
                RaisePropertyChanged();
            } 
        }

        public bool CanShare()
        {
            return SelectedUser != null && SelectedWallet != null;
        }
        public async void Share()
        {
            IsEnabled = false;
            var shared = await RelationService<User, Wallet>.AddConnection(SelectedUser, SelectedWallet);
            if (shared)
            {
                MessageBox.Show("Successfully shared wallet");
            }
            else
            {
                MessageBox.Show("User already owns this wallet");
            }
            IsEnabled = true;
        }
        public User SelectedUser { 
            get { return _selectedUser; } 
            set { 
                _selectedUser = value;
                ShareCommand.RaiseCanExecuteChanged();
            } 
        }
        public Wallet SelectedWallet { 
            get { return _selectedWallet; } 
            set { 
                _selectedWallet = value;
                ShareCommand.RaiseCanExecuteChanged();
            } 
        }

        public List<User> Users => _users;
        public List<Wallet> Wallets => _user.Wallets.ToList();

        public NavigatableType Type => NavigatableType.Share;
    }
}
