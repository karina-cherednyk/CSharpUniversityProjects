using Budgets.BusinessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Windows.Controls;


namespace BudgetsWPF.Transactions
{

    public partial class TransactionsView : UserControl
    {
        private TransactionsViewModel _viewModel;
        public TransactionsView(User user, Wallet wallet, Action goToWallets)
        {
            InitializeComponent();
            _viewModel = new TransactionsViewModel(user, wallet, goToWallets);
            DataContext = _viewModel;
        }
    }
}
