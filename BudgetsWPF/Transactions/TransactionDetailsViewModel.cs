
using Budgets.BusinessLayer.Entities;
using Budgets.Common;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetsWPF.Transactions
{
    public class TransactionDetailsViewModel : BindableBase
    {
        private Transaction _transaction;
        private Wallet _wallet;
        public DelegateCommand SaveTransactionCommand { get; }

        public TransactionDetailsViewModel(Wallet wallet, Transaction transaction)
        {
            _transaction = transaction;
            _wallet = wallet;
            _sum = _transaction.Sum.ToString("F");
            _selectedCurrency = CurrencyConvertor.CurencyToString(_transaction.Currency);
            
            // TODO
            SaveTransactionCommand = new DelegateCommand(() => { });
        }

        public string User => _transaction.User.FullName;

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

        public List<string> Currencies
        {
            get
            {
                return CurrencyConvertor.AllCurrencies();
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

            }
        }


    }
}
