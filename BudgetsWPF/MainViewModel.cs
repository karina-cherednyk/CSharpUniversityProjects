using Budgets.BusinessLayer.Entities;
using BudgetsWPF.Authentication;
using BudgetsWPF.Categories;
using BudgetsWPF.Navigation;
using BudgetsWPF.Transactions;
using BudgetsWPF.Wallets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetsWPF
{
    public static class MainNavigator
    { 
        private static NavigationBase _base;
        public static NavigationBase NavigationBase {  
            set
            {
                    _base = value;
            } 
        }
        private static INavigatable GetNavigatable(NavigatableType type, params Object[] args)
        {
            switch (type)
            {
                case NavigatableType.SignIn: return new SignInViewModel();
                case NavigatableType.SignUp: return new SignUpViewModel();
                case NavigatableType.Wallets: return new WalletsViewModel((User)args[0]);
                case NavigatableType.Categories: return new CategoriesViewModel((User)args[0]);
                case NavigatableType.Transactions: return new TransactionsViewModel((User)args[0], (Wallet)args[1]);
                default: throw new NotImplementedException();
            }
        }
        public static void Navigate(NavigatableType type, params Object[] args)
        {
             _base.Navigate(GetNavigatable(type, args));

        }

    }


    public class MainViewModel : NavigationBase
    {
        public MainViewModel()
        {
            MainNavigator.NavigationBase = this;
            Navigate(new SignInViewModel());
        }


    }
}
