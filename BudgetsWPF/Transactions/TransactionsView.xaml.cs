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

        private static Regex numRegex = new Regex("[^0-9]+");
        public TransactionsView()
        {
            InitializeComponent();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = numRegex.IsMatch(e.Text);
        }
    }
}
