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

    public partial class SignInView : UserControl
    {
        AuthenticationService authService;
        Action toSignUp;
        Action<DBUser> toWallets;
        private SignInViewModel _viewModel;

        public SignInView(Action goToSignUp, Action<DBUser> goToWallets)
        {
            InitializeComponent();
            _viewModel = new SignInViewModel(goToSignUp, goToWallets);
            /*            authService = new AuthenticationService();
                        toSignUp = goToSignUpView;
                        toWallets = goToWalletsView;*/
            this.DataContext = _viewModel;
        }



       /* async private void DoSignIn(object sender, RoutedEventArgs e)
        {
            var login = LoginInput.Text.Trim();
            var pass = PassInput.Password.Trim();

            this.IsEnabled = false;
            DBUser u = new DBUser(login, null, pass);
            try
            {
                var user = await authService.AuthenticateAsync(u);
                toWallets(user);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.IsEnabled = true;

        }
        private void ToSignUpView(object sender, RoutedEventArgs e)
        {
            toSignUp();
        }*/
    }
}
