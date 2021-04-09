using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Budgets.BusinessLayer.Entities;
using BugetsStorage.Services;
using Prism.Commands;

namespace BudgetsWPF.Authentication
{
    public class SignInViewModel : INotifyPropertyChanged
    {

        private readonly Action _goToSignUp;
        private readonly Action<User> _goToWallets;

        public DelegateCommand SignInCommand { get; }
        public DelegateCommand SignUpCommand { get; }


        public SignInViewModel(Action goToSignUp, Action<User> goToWallets)
        {
            _goToSignUp = goToSignUp;
            _goToWallets = goToWallets;

            SignInCommand = new DelegateCommand(SignIn, IsSignInEnabled);
            SignUpCommand = new DelegateCommand(_goToSignUp);
        }


        private bool IsSignInEnabled()
        {
            return !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password);
        }

        async private void SignIn()
        {
            if(string.IsNullOrWhiteSpace(Login) ||
                string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Login or password is empty");
            }
            else
            {
                try
                {
                    var user = await AuthUserService.AuthenticateAsync(_authUser);
                    _goToWallets.Invoke(user);
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"Sign In failed: {ex.Message}");
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private AuthUser _authUser = new AuthUser();
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
