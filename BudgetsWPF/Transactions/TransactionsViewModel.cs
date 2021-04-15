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
        public ObservableCollection<TransactionDetailsViewModel> Transactions { get; }
        private TransactionDetailsViewModel _currentTransaction;
        public DelegateCommand ToWalletsCommand { get;  }
        public DelegateCommand AddTransactionCommand { get; }

        private User _user;
        private Wallet _wallet;
        

        public TransactionsViewModel(User user, Wallet wallet)
        {
            _user = user;
            _wallet = wallet;

            Transactions = new();
            ToWalletsCommand = new DelegateCommand(() => MainNavigator.Navigate(NavigatableType.Wallets, _user));
            AddTransactionCommand = new DelegateCommand(AddTransaction, CanAddTransaction);
            foreach(var transaction in wallet.Transactions)
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
            RaisePropertyChanged(nameof(CurrentTransaction));
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
            }
        }
        private uint _loadFrom = 0;
        public string LoadFrom
        {
            get
            {
                return _loadFrom.ToString();
            }
            set
            {
                _loadFrom  = uint.Parse(value);
            }
        }

        public NavigatableType Type => NavigatableType.Transactions;
    }
}
