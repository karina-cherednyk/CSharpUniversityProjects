using Budgets.BusinessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace BudgetsWPF.ShareUserWallet
{
    public partial class ShareView : UserControl
    {
        public ShareViewModel _viewModel;
        public ShareView(User user, List<User> users, Action<User> goToWallets)
        {
            InitializeComponent();
            _viewModel = new ShareViewModel(user, users, goToWallets);
            DataContext = _viewModel;
        }
    }
}
