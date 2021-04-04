using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using BudgetsWPF.Authentication;

namespace BudgetsWPF.Wallets
{
    public partial class WalletsView : UserControl
    {

        private WalletsViewModel _viewModel;
        public WalletsView(DBUser user, Action goToSignInView)
        {
            InitializeComponent();
            _viewModel = new WalletsViewModel();
            DataContext = _viewModel;
        }
    }
}
