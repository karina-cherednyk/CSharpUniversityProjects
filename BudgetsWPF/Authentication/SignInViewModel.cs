using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Budgets.BusinessLayer.Entities;
using BudgetsStorage.Services;
using BudgetsWPF.Navigation;
using Prism.Commands;

namespace BudgetsWPF.Authentication
{
    public class SignInViewModel : INotifyPropertyChanged, INavigatable 
    {

        public DelegateCommand SignInCommand { get; }
        public DelegateCommand SignUpCommand { get; }


        public SignInViewModel()
        {

            SignInCommand = new DelegateCommand(SignIn, IsSignInEnabled);
            SignUpCommand = new DelegateCommand(() => MainNavigator.Navigate(NavigatableType.SignUp));
        }

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            private set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool IsSignInEnabled()
        {
            return !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password);
        }

        async private void SignIn()
        {
            IsEnabled = false;
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
                    MainNavigator.Navigate(NavigatableType.Wallets, user);
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"Sign In failed: {ex.Message}");
                }
            }
            IsEnabled = true;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private SignInUser _authUser = new SignInUser();
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

        public NavigatableType Type => NavigatableType.SignIn;

    }
}
