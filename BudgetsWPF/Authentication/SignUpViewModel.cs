using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Budgets.BusinessLayer.Entities;
using Budgets.Common;
using BudgetsStorage.Services;
using BudgetsWPF.Navigation;
using Prism.Commands;

namespace BudgetsWPF.Authentication
{
    internal class SignUpViewModel: INotifyPropertyChanged, INavigatable
    {

        private readonly SignUpUser _regUser = new SignUpUser();

        public DelegateCommand SignUpCommand { get; }
        public DelegateCommand SignInCommand { get; }


        public SignUpViewModel()
        {


            SignUpCommand = new DelegateCommand(SignUp, IsSignUpEnabled);
            SignInCommand = new DelegateCommand(() => MainNavigator.Navigate(NavigatableType.SignIn));
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

        async private void SignUp()
        {
            IsEnabled = false;
           try
            {
                var user = await AuthUserService.RegisterUserAsync(_regUser);
                MessageBox.Show($"User {_regUser.Login} succesfully registered");
                MainNavigator.Navigate(NavigatableType.Wallets, user);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Sign In failed: {ex.Message}");
            }
            IsEnabled = true;

        }
        private bool IsSignUpEnabled()
        {
            return
                !string.IsNullOrWhiteSpace(Login) &&
                !string.IsNullOrWhiteSpace(Password) &&
                !string.IsNullOrWhiteSpace(FirstName) &&
                !string.IsNullOrWhiteSpace(LastName) &&
                Validator.ValidateEmail(Email) &&
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
        public string FirstName
        {
            get { return _regUser.FirstName;  }
            set
            {
                if (_regUser.FirstName != value)
                {
                    _regUser.FirstName = value;
                    OnPropertyChanged();
                    SignUpCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string LastName
        {
            get { return _regUser.LastName;  }
            set
            {
                if(_regUser.LastName != value)
                {
                    _regUser.LastName = value;
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

        public NavigatableType Type => NavigatableType.SignUp;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}