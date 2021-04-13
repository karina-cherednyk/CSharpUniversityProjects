using Budgets.BusinessLayer.Entities;
using Budgets.Common;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using BudgetsStorage.Services;

namespace BudgetsWPF.Transactions
{
    public class TransactionDetailsViewModel : BindableBase
    {
        private static int newCount = 0;
        private Transaction _transaction;
        private Wallet _wallet;
        private User _owner;
        private User _currentUser;
        private int _displayIndex;
        private Action<TransactionDetailsViewModel> _removeTransactionView;
        public DelegateCommand SaveTransactionCommand { get; }
        public DelegateCommand RemoveTransactionCommand { get; }

        public string DisplayName => _wallet.Name + (_transaction.IsNew ? " new " : " ") + $"#{_displayIndex}";

        public TransactionDetailsViewModel(User user, Wallet wallet, Transaction transaction, Action<TransactionDetailsViewModel> removeTransactionView)
        {
            _transaction = transaction;
            _wallet = wallet;
            _owner = transaction.Owner;
            _currentUser = user;
            _sum = _transaction.Sum.ToString("F");
            _selectedCurrency = CurrencyConvertor.CurencyToString(_transaction.Currency);
            _removeTransactionView = removeTransactionView;
            
            SaveTransactionCommand = new DelegateCommand(SaveTransaction, CanSaveTransaction);
            RemoveTransactionCommand = new DelegateCommand(RemoveTransaction, CanRemoveTransaction);

            if (!_transaction.IsNew) _displayIndex = _wallet.Transactions.IndexOf(_transaction) + 1;
            else _displayIndex = ++newCount;
        }

        public string User => _transaction.Owner.FullName;

        public bool CanSaveTransaction() => _transaction.HasChanges && _transaction.IsValid;
        public bool CanRemoveTransaction() => !_transaction.IsNew && _owner.Equals( _currentUser);

        public async void RemoveTransaction()
        {
            await RelationService<Wallet, Transaction>.RemoveConnection(_wallet, _transaction);
            _wallet.RemoveTransaction(_transaction);
            _removeTransactionView(this);
        }
        public async void SaveTransaction()
        {
            await TransactionService.Add(_transaction);
            await RelationService<Wallet, Transaction>.AddConnection(_wallet, _transaction);
            _transaction.HasChanges = false;
            _transaction.IsNew = false;
            RemoveTransactionCommand.RaiseCanExecuteChanged();
            SaveTransactionCommand.RaiseCanExecuteChanged();

        }

        public DateTime Date
        {
            get { return _transaction.Date; }
            set { _transaction.Date = value; }
        }

        private string _sum;
        public string Sum
        {
            get { return _sum; }
            set
            {
                _sum = value;
            }
        }

        public Category SelectedCategory
        {
            get
            {
                return _transaction.Category;
            }
            set
            {
                _transaction.Category = value;
            }
        }

        public List<Category> Categories => _wallet.Categories.ToList();
        public List<string> Currencies
        {
            get
            {
                return CurrencyConvertor.AllCurrencies();
            }
        }


        public void SetSum()
        {
            decimal value;
            if (decimal.TryParse(_sum, out value))
            {
                Sum = value.ToString("F");
                _transaction.Sum = value;
            }
            else
                Sum = string.Empty;
        }

        public string Description
        {
            get { return _transaction.Description; }
            set { _transaction.Description = value; }
        }


        private string _selectedCurrency;
        public string SelectedCurrency
        {
            get
            {
                return _selectedCurrency;
            }
            set
            {
                _selectedCurrency = value;
                _transaction.Currency = CurrencyConvertor.CurrencyFromString(value);

                RaisePropertyChanged();
                Sum = _transaction.Sum.ToString("F");
                RaisePropertyChanged(nameof(Sum));

            }
        }


    }
}
