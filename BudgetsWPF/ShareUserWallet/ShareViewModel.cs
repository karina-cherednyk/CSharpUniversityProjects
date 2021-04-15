using Budgets.BusinessLayer.Entities;
using BudgetsStorage.Services;
using BudgetsWPF.Navigation;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BudgetsWPF.ShareUserWallet
{
    public class ShareViewModel
    {
        private User _user;
        private List<User> _users;
        public DelegateCommand ShareCommand { get; }
        public DelegateCommand ToWalletsCommand { get; }
        public ShareViewModel(User user, List<User> users, Action<User> goToWallets)
        {
            _user = user;
            _users = users;
            ToWalletsCommand = new DelegateCommand(() => MainNavigator.Navigate(NavigatableType.Wallets, _user));
            ShareCommand = new DelegateCommand(Share);
        }
        public async void Share()
        {
            if(SelectedUser == null && SelectedCategory == null)
            {
                MessageBox.Show("Choose user and category!");
                return;
            }
            if (SelectedUser == null)
            {
                MessageBox.Show("Choose user!");
                return;
            }

            if (SelectedCategory == null)
            {
                MessageBox.Show("Choose category!");
                return;
            }
            
            var shared = await RelationService<User, Category>.AddConnection(SelectedUser, SelectedCategory);
            if (shared)
            {
                MessageBox.Show("Successfully shared wallet");
            }
            else
            {
                MessageBox.Show("User already owns this wallet");
            }
        }
        public User SelectedUser { get; set; }
        public Category SelectedCategory { get; set; }

        public List<User> Users => _users;
        public List<Category> Categories => _user.Categories.ToList();

    }
}
