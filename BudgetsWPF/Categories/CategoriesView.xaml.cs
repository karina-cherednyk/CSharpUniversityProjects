using Budgets.BusinessLayer.Entities;

using System;
using System.Windows.Controls;


namespace BudgetsWPF.Categories
{

    public partial class CategoriesView : UserControl
    {
        private CategoriesViewModel _viewModel;
        public CategoriesView(User user, Action<User> goToWalletsView)
        {
            InitializeComponent();
            _viewModel = new CategoriesViewModel(user, goToWalletsView);
            DataContext = _viewModel;
        }
    }
}
