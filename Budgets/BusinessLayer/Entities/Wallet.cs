using System.Collections.Generic;
using Budgets.Common;
using System;
using System.Text.Json.Serialization;

namespace Budgets.BusinessLayer.Entities
{
	public class Wallet : BaseEntity, ICategorizable
    {
        private string _name, _description;
        private decimal _initBalance;
        private Currency _currency;
        private HashSet<Category> _categories;
        private List<Transaction> _transactions;

        public Guid Owner { get;  }
        public string Name { get { return _name; } set { _name = value; HasChanges = true; } }
        public decimal InitialBalance { get { return _initBalance; } set { _initBalance = value; HasChanges = true; } }
        public string Description { get { return _description; } set { _description = value; HasChanges = true; } }

        [JsonIgnore]
        public List<Transaction> Transactions => _transactions== null ? new () : _transactions;
        [JsonIgnore]
        public HashSet<Category> Categories => _categories;




        public Currency Currency
        {
            get { return _currency; }
            set
            {
                InitialBalance = CurrencyConvertor.convert(InitialBalance, _currency, value);
                _currency = value;
                HasChanges = true;
            }
        }
        [JsonConstructor]
        public Wallet(Guid id, Guid owner, string name, string description, 
            decimal initialBalance, Currency currency, 
            decimal balance, decimal monthLoss, decimal monthProfit )
        {
            Id = id;
            Owner = owner;
            Name = name;
            Description = description;
            InitialBalance = initialBalance;
            Currency = currency;
            _categories = new();
            //_transactions = new();
            _balance = balance;
            _monthLoss = monthLoss;
            _monthProfit = monthProfit;
        }
        public Wallet(Guid owner, string name, string description, decimal initialBalance, Currency currency):
            this(Guid.NewGuid(), owner, name, description, initialBalance, currency, 0, 0, 0)
        {
            IsNew = true;
        }
        private decimal _balance;
        private decimal _monthLoss;
        private decimal _monthProfit;

        
        public decimal Balance
        {
            get {
                if (_transactions != null)
                {
                    decimal sum = InitialBalance;
                    foreach (Transaction t in _transactions)
                    {
                        sum += CurrencyConvertor.convert(t.Sum, t.Currency, _currency);
                    }

                    return sum;
                }
                else return _balance;
            }

        }
        
        public decimal MonthLoss
        {
            get
            {
                if (_transactions != null)
                {
                    DateTime nowD = DateTime.Now;
                    decimal sum = 0;
                    TimeSpan ts;
                    foreach (Transaction t in _transactions)
                    {
                        ts = nowD.Subtract(t.Date);
                        if (t.Sum < 0 & ts.TotalDays < 32)
                        {
                            sum -= CurrencyConvertor.convert(t.Sum, t.Currency, _currency);
                        }
                    }

                    return sum;
                }
                else return _monthLoss;
            }
        }
      
        public decimal MonthProfit
        {
            get
            {
                if (_transactions != null)
                {
                    DateTime nowD = DateTime.Now;
                    decimal sum = 0;
                    TimeSpan ts;
                    foreach (Transaction t in _transactions)
                    {
                        ts = nowD.Subtract(t.Date);
                        if (t.Sum > 0 & ts.TotalDays < 32)
                        {
                            sum += CurrencyConvertor.convert(t.Sum, t.Currency, _currency);
                        }
                    }

                    return sum;
                }
                else return _monthProfit;
            }

        }
        public bool HasCategory(Category category) => _categories.Contains(category);
        public bool AddCategory(Category category)
        {
            if (_categories.Add(category)){
                HasChanges = true;
                return true;
            }
            return false;
        }
        public bool RemoveCategory(Category category)
        {
            if (_categories.Remove(category))
            {
                HasChanges = true;
                return true;
            }
            return false;
        }
        public bool AddTransaction(Transaction transaction)
        {
            if (_transactions == null) _transactions = new(); 
            if (
                _transactions.Contains(transaction) ||
                (transaction.Owner != null && !transaction.Owner.HasWallet(this)) ||
                !_categories.Contains(transaction.Category)
                ) return false;

            _transactions.Add(transaction);
            return true;
        }

        public bool RemoveTransaction(Transaction transaction) => _transactions.Remove(transaction);

        public override bool Validate()
        {
            return
                !string.IsNullOrWhiteSpace(Name) &
                !string.IsNullOrWhiteSpace(Description) &
                Balance >= 0;

        }
    }
}