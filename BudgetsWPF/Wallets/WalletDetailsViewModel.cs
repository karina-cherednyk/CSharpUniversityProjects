using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Budgets.BusinessLayer.Entities;
using Budgets.Common;
using BugetsStorage.Services;
using Prism.Commands;
using Prism.Mvvm;

namespace BudgetsWPF.Wallets
{
    class WalletDetailsViewModel: BindableBase
    {
        public Wallet Wallet { get; }
        public ObservableCollection<Category> Categories;
        private string _selectedCurrency;
        public DelegateCommand RemoveWalletCommand { get; }
        public DelegateCommand ShowTransactionsCommand { get; }
        public DelegateCommand SaveWalletCommand { get;  }

        public WalletDetailsViewModel(User user,  Wallet wallet, Action<WalletDetailsViewModel> removeWallet, Action<User, Wallet> goToTransactions)
        {
            Wallet = wallet;
            Categories = new(wallet.Categories);
            _selectedCurrency = CurrencyConvertor.CurencyToString(Wallet.Currency);
            RemoveWalletCommand = new DelegateCommand( () => removeWallet(this));
            ShowTransactionsCommand = new DelegateCommand(() => goToTransactions(user, Wallet));
            SaveWalletCommand = new DelegateCommand(async () => { });
        }


        public List<string> Currencies
        {
            get
            {
                return CurrencyConvertor.AllCurrencies();
            }
        }
        

        public string SelectedCurrency
        {
            get
            {
                return _selectedCurrency;
            }
            set
            {
                _selectedCurrency = value;
                Wallet.Currency = CurrencyConvertor.CurrencyFromString(value);

                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Balance));
                RaisePropertyChanged(nameof(MonthLoss));
                RaisePropertyChanged(nameof(MonthProfit));
            }
        }


        public string DisplayName
        {
            get
            {
                return $"{Wallet.Name} ({Balance}{CurrencyConvertor.CurencyToSign(Wallet.Currency)})";
            }
        }

 
        public string Name
        {
            get
            {
                return Wallet.Name;
            }
            set
            {
                Wallet.Name = value;
                RaisePropertyChanged(nameof(DisplayName));
            }
        }

        public string Balance => Wallet.Balance.ToString("F");
        public string MonthLoss => Wallet.MonthLoss.ToString("F");
        public string MonthProfit => Wallet.MonthProfit.ToString("F");

    }
}
