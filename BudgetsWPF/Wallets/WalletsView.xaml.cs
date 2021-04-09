using System;
using System.Windows.Controls;
using Budgets.BusinessLayer.Entities;


namespace BudgetsWPF.Wallets
{
    public partial class WalletsView : UserControl
    {

        private WalletsViewModel _viewModel;
        public WalletsView(User user, Action goToSignInView)
        {
            InitializeComponent();
            _viewModel = new WalletsViewModel(user, goToSignInView);
            DataContext = _viewModel;
        }
    }
}
