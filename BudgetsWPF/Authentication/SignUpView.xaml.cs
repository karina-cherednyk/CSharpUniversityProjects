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
    public partial class SignUpView : UserControl
    {

        private SignUpViewModel _viewModel;

        public SignUpView()
        {
            InitializeComponent();
            _viewModel = new SignUpViewModel();
            DataContext = _viewModel;
        }
        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.Password = Password.Password;
        }
        private void OnPassword2Changed(object sender, RoutedEventArgs e)
        {
            _viewModel.Password2 = Password2.Password;
        }

    }
}
