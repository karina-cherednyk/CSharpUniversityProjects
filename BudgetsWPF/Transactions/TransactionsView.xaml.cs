using Budgets.BusinessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace BudgetsWPF.Transactions
{

    public partial class TransactionsView : UserControl
    {
        private TransactionsViewModel _viewModel;
        private static Regex numRegex = new Regex("[^0-9]+");
        public TransactionsView(User user, Wallet wallet, Action goToWallets)
        {
            InitializeComponent();
            _viewModel = new TransactionsViewModel(user, wallet, goToWallets);
            DataContext = _viewModel;
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = numRegex.IsMatch(e.Text);
        }
    }
}
