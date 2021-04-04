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
    /// <summary>
    /// Interaction logic for SignUpView.xaml
    /// </summary>
    public partial class SignUpView : UserControl
    {
        AuthenticationService authService;
        Action toSignIn;
        Action<DBUser> toWallets;


        public SignUpView(Action goToSignInView, Action<DBUser> goToWalletsView)
        {
            InitializeComponent();
            authService = new AuthenticationService();
            toSignIn = goToSignInView;
            toWallets = goToWalletsView;
        }
        private bool checkInput(Control box, bool correct, string error)
        {
            if (correct)
            {
                box.ToolTip = "";
                box.Background = Brushes.Transparent;
            }
            else
            {
                box.ToolTip = error;
                box.Background = Brushes.LightPink;
            }
            return correct;
        }

        async private void DoSignUp(object sender, RoutedEventArgs e)
        {
            var allCorrect = true;
            var login = LoginInput.Text.Trim();
            var pass = PassInput.Password.Trim();
            var pass2 = PassRepeatInput.Password.Trim();
            var email = EmailInput.Text.Trim();


            allCorrect &= checkInput(LoginInput,
                login.Length > 1, "Login length > 1");
            allCorrect &= checkInput(EmailInput,
                email.Contains("@"),
                "Incorrect email");
            allCorrect &= checkInput(PassInput,
                pass.Length > 1,
                "Password length > 1");
            allCorrect &= checkInput(PassRepeatInput,
                pass == pass2,
                "Passwords not the same");



            if (allCorrect)
            {
                this.IsEnabled = false;
                DBUser u = new DBUser(login, email, pass);
                try
                {
                    await authService.RegisterUserAsync(u);
                    toWallets(u);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                this.IsEnabled = true;
            }

        }

        private void ToSignInView(object sender, RoutedEventArgs e)
        {
            toSignIn();
        }
    }
}
