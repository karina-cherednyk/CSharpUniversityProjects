using Budgets.BusinessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BudgetsWPF.Authentication
{

    public partial class SignInView : UserControl
    {
        private SignInViewModel _viewModel;

        public SignInView()
        {
            InitializeComponent();
            _viewModel = new SignInViewModel();
            DataContext = _viewModel;
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.Password = Password.Password;
        }
    }

}
