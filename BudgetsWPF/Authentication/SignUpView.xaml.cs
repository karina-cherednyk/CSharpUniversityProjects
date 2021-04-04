using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BudgetsWPF.Authentication
{
    public partial class SignUpView : UserControl
    {

        private SignUpViewModel _viewModel;

        public SignUpView(Action goToSignIn, Action<DBUser> goToWallets)
        {
            InitializeComponent();
            _viewModel = new SignUpViewModel(goToSignIn, goToWallets);
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
