using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Budgets.BusinessLayer.Entities;
using BugetsStorage.Services;
using Prism.Commands;

namespace BudgetsWPF.Authentication
{
    internal class SignUpViewModel: INotifyPropertyChanged
    {
        private readonly Action _goToSignIn;
        private readonly Action<User> _goToWallets;
        private readonly AuthUser _regUser = new AuthUser();

        public DelegateCommand SignUpCommand { get; }
        public DelegateCommand SignInCommand { get; }


        public SignUpViewModel(Action goToSignIn, Action<User> goToWallets)
        {
            _goToSignIn = goToSignIn;
            _goToWallets = goToWallets;

            SignUpCommand = new DelegateCommand(SignUp, IsSignUpEnabled);
            SignInCommand = new DelegateCommand(_goToSignIn);
        }

        async private void SignUp()
        {
           try
            {
                var user = await AuthUserService.RegisterUserAsync(_regUser);
                MessageBox.Show($"User {_regUser.Login} succesfully registered");
                _goToWallets.Invoke(user);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Sign In failed: {ex.Message}");
            }

        }
        private bool IsSignUpEnabled()
        {
            return
                !string.IsNullOrWhiteSpace(Login) &&
                !string.IsNullOrWhiteSpace(Password) &&
                !string.IsNullOrWhiteSpace(Email) &&
                Password2 == Password;
        }

        public string Login
        {
            get { return _regUser.Login;  }
            set
            {
                if(_regUser.Login != value)
                {
                    _regUser.Login = value;
                    OnPropertyChanged();
                    SignUpCommand.RaiseCanExecuteChanged();
                }
            }
        }
        public string Password
        {
            get { return _regUser.Password; }
            set
            {
                if(_regUser.Password != value)
                {
                    _regUser.Password = value;
                    OnPropertyChanged();
                    SignUpCommand.RaiseCanExecuteChanged();
                }
            }
        }
        public string Email
        {
            get { return _regUser.Email; }
            set
            {
                if (_regUser.Email != value)
                {
                    _regUser.Email = value;
                    OnPropertyChanged();
                    SignUpCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string password2;
        public string Password2
        {
            get { return password2; }
            set
            {
                password2 = value;
                OnPropertyChanged();
                SignUpCommand.RaiseCanExecuteChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}