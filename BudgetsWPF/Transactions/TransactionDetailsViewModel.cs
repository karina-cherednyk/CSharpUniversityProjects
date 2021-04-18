using Budgets.BusinessLayer.Entities;
using Budgets.Common;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using BudgetsStorage.Services;
using System.Threading;

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
            
            _selectedCurrency = CurrencyConvertor.CurencyToString(_transaction.Currency);
            _removeTransactionView = removeTransactionView;
            
            SaveTransactionCommand = new DelegateCommand(() => new Thread(() => SaveTransaction()).Start(), CanSaveTransaction);
            RemoveTransactionCommand = new DelegateCommand(() => new Thread(() => RemoveTransaction()).Start(), CanRemoveTransaction);

            if (!_transaction.IsNew) _displayIndex = _wallet.Transactions.IndexOf(_transaction) + 1;
            else _displayIndex = ++newCount;

            IsEnabled = user == _owner;
        }

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            private set
            {
                _isEnabled = value;
                RaisePropertyChanged();
            }
        }



        public string User => _owner.FullName;

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
            _wallet.AddTransaction(_transaction);
            await TransactionService.Add(_transaction);
            await WalletService.Add(_wallet);
            await RelationService<Wallet, Transaction>.AddConnection(_wallet, _transaction);

            _transaction.HasChanges = false;
            _transaction.IsNew = false;
            
            RemoveTransactionCommand.RaiseCanExecuteChanged();
            SaveTransactionCommand.RaiseCanExecuteChanged();

        }

        public DateTime Date
        {
            get { return _transaction.Date; }
            set { 
                _transaction.Date = value; 
                SaveTransactionCommand.RaiseCanExecuteChanged(); 
            }
        }

        public string Sum
        {
            get { return _transaction.Sum.ToString("F"); }
            set
            {
                decimal val;
                if (decimal.TryParse(value, out val))
                {
                    _transaction.Sum = val;
                    SaveTransactionCommand.RaiseCanExecuteChanged();
                }
                else
                    _transaction.Sum = 0;
                RaisePropertyChanged();
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
                SaveTransactionCommand.RaiseCanExecuteChanged();
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

        public string Description
        {
            get { return _transaction.Description; }
            set { 
                _transaction.Description = value;
                SaveTransactionCommand.RaiseCanExecuteChanged();
            }
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
                SaveTransactionCommand.RaiseCanExecuteChanged();
            }
        }


    }
}
