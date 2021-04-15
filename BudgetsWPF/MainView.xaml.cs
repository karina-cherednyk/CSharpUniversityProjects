using System.Windows.Controls;
using Budgets.BusinessLayer.Entities;
using BudgetsWPF.Authentication;
using BudgetsWPF.Transactions;
using BudgetsWPF.Categories;
using BudgetsWPF.Wallets;
using System.Collections.Generic;
using BudgetsWPF.ShareUserWallet;

namespace BudgetsWPF
{
    public partial class MainView : UserControl
    {
        MainViewModel _viewModel;
        public MainView()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel;
        }
 
    }
}
