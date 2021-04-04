using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Prism.Commands;

namespace BudgetsWPF.Authentication
{
    public class SignInViewModel : INotifyPropertyChanged
    {

        private readonly AuthenticationService _authService = new AuthenticationService();
        private readonly Action _goToSignUp;
        private readonly Action<DBUser> _goToWallets;

        public DelegateCommand SignInCommand { get; }
        public DelegateCommand SignUpCommand { get; }


        public SignInViewModel(Action goToSignUp, Action<DBUser> goToWallets)
        {
            _goToSignUp = goToSignUp;
            _goToWallets = goToWallets;

            SignInCommand = new DelegateCommand(SignIn, IsSignInEnabled);
            SignUpCommand = new DelegateCommand(_goToSignUp);
        }


        private bool IsSignInEnabled()
        {
            return !String.IsNullOrWhiteSpace(Login) && !String.IsNullOrWhiteSpace(Password);
        }

        async private void SignIn()
        {
            if(String.IsNullOrWhiteSpace(Login) ||
                String.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Login or password is empty");
            }
            else
            {
                DBUser user;
                try
                {
                    user = await _authService.AuthenticateAsync(_authUser);
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"Sign In failed: {ex.Message}");
                    return;
                }
                MessageBox.Show($"Sign In was successful for user {user.Login}");
                _goToWallets.Invoke(user);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private DBUser _authUser = new DBUser();
        public string Login
        {
            get
            {
                return _authUser.Login;
            }
            set
            {
                if (_authUser.Login != value)
                {
                    _authUser.Login = value;
                    OnPropertyChanged();
                    SignInCommand.RaiseCanExecuteChanged();
                }
            }
        }
        public string Password
        {
            get {   return _authUser.Password; }
            set
            {
                if(_authUser.Password != value)
                {
                    _authUser.Password = value;
                    OnPropertyChanged();
                    SignInCommand.RaiseCanExecuteChanged();
                }
            }
        }
    }
}
