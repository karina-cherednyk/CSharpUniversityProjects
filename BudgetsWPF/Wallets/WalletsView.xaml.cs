using System;
using System.Windows.Controls;
using Budgets.BusinessLayer.Entities;


namespace BudgetsWPF.Wallets
{
    public partial class WalletsView : UserControl
    {

        private WalletsViewModel _viewModel;
        public WalletsView(User user, Action goToSignInView, Action<User, Wallet> goToTransactionsView, Action<User> gotoCategoriesView)
        {
            InitializeComponent();
            _viewModel = new WalletsViewModel(user, goToSignInView, goToTransactionsView, gotoCategoriesView);
            DataContext = _viewModel;
        }
    }
}
