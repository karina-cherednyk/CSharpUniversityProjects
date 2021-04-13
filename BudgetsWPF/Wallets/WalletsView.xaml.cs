using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Budgets.BusinessLayer.Entities;


namespace BudgetsWPF.Wallets
{
    public partial class WalletsView : UserControl
    {

        private WalletsViewModel _viewModel;
        public WalletsView(User user, Action goToSignInView, Action<User, Wallet> goToTransactionsView, Action<User> gotoCategoriesView, Action<User, List<User>> goToShareView)
        {
            InitializeComponent();
            _viewModel = new WalletsViewModel(user, goToSignInView, goToTransactionsView, gotoCategoriesView, goToShareView);
            DataContext = _viewModel;
        }
    }
}
