using Budgets.BusinessLayer.Entities;
using BudgetsWPF.Navigation;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;


namespace BudgetsWPF.Transactions
{
    public class TransactionsViewModel: BindableBase, INavigatable
    {
        public ObservableCollection<TransactionDetailsViewModel> Transactions { get; set;  }
        private TransactionDetailsViewModel _currentTransaction;
        public DelegateCommand ToWalletsCommand { get;  }
        public DelegateCommand AddTransactionCommand { get; }

        private User _user;
        private Wallet _wallet;
        

        public TransactionsViewModel(User user, Wallet wallet)
        {
            _user = user;
            _wallet = wallet;

            Transactions = new ObservableCollection<TransactionDetailsViewModel>();
            ToWalletsCommand = new DelegateCommand(() => MainNavigator.Navigate(NavigatableType.Wallets, _user));
            AddTransactionCommand = new DelegateCommand(AddTransaction, CanAddTransaction);
            foreach(var transaction in wallet.LoadFrom(0))
            {
                Transactions.Add(new TransactionDetailsViewModel(user, wallet, transaction, RemoveTransactionView));
            }

        }

        public bool CanAddTransaction()
        {
            return _wallet.Categories.Count > 0;
        }

        public void AddTransaction()
        {
            Transaction t = new Transaction(_user, 0, _wallet.Currency, null, "");
            _wallet.AddTransaction(t);
            Transactions.Add(new TransactionDetailsViewModel(_user, _wallet, t, RemoveTransactionView));
            RaisePropertyChanged(nameof(Transactions));
        }

        public void RemoveTransactionView(TransactionDetailsViewModel td)
        {
            Transactions.Remove(td);
            RaisePropertyChanged(nameof(CurrentTransaction));
            RaisePropertyChanged(nameof(Transactions));
        }

        public TransactionDetailsViewModel CurrentTransaction
        {
            get
            {
                return _currentTransaction;
            }
            set
            {
                _currentTransaction = value;
                RaisePropertyChanged();
            }
        }
        private int _loadFrom = 1;
        public string LoadFrom
        {
            get
            {
                return _loadFrom.ToString();
            }
            set
            {
                int val;
                if (int.TryParse(value, out val))
                {
                    if (_loadFrom == val) return;
                    _loadFrom = val;
                    Transactions.Clear();
                    foreach (var transaction in _wallet.LoadFrom(_loadFrom-1))
                    {
                        Transactions.Add(new TransactionDetailsViewModel(_user, _wallet, transaction, RemoveTransactionView));
                    }

                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(CurrentTransaction));
                    RaisePropertyChanged(nameof(Transactions));
                }
            }
        }

        public NavigatableType Type => NavigatableType.Transactions;
    }
}
