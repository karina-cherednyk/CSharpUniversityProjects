using System.Collections.Generic;
using Budgets.Common;
using System;
using System.Text.Json.Serialization;

namespace Budgets.BusinessLayer.Entities
{
	public class Wallet : BaseEntity, ICategorizable
    {

        public Guid Owner { get;  }
        public string Name { get; set; }
        public double InitialBalance { get; private set; }
        public string Description { get; set; }

        [JsonIgnore]
        public List<Transaction> Transactions => _transactions;
        [JsonIgnore]
        public HashSet<Category> Categories => _categories;


        private Currency _currency;
        private HashSet<Category> _categories;
        private List<Transaction> _transactions;

        public Currency Currency
        {
            get { return _currency; }
            set
            {
                InitialBalance = Convertor.convert(InitialBalance, _currency, value);
                _currency = value;
            }
        }
        [JsonConstructor]
        public Wallet(Guid id, Guid owner, string name, string description, double initialBalance, Currency currency )
        {
            Id = id;
            Owner = owner;
            Name = name;
            Description = description;
            InitialBalance = initialBalance;
            Currency = currency;
            _categories = new();
            _transactions = new();
        }
        public Wallet(Guid owner, string name, string description, double initialBalance, Currency currency):
            this(Guid.NewGuid(), owner, name, description, initialBalance, currency)
        {}


        [JsonIgnore]
        public double Balance
        {
            get {
                double sum = InitialBalance;
                foreach(Transaction t in _transactions)
                {
                     sum += Convertor.convert(t.Sum, t.Currency, _currency);
                }

                return sum;
            }
        }
        [JsonIgnore]
        public double MonthLoss
        {
            get
            {
                DateTime nowD = DateTime.Now;
                double sum = 0;
                TimeSpan ts;
                foreach (Transaction t in _transactions)
                {
                    ts = nowD.Subtract(t.Date);
                    if(t.Sum < 0 & ts.TotalDays < 32)
                    {
                        sum -= Convertor.convert(t.Sum, t.Currency, _currency);
                    }
                }

                return sum;
            }
        }
        [JsonIgnore]
        public double MonthProfit
        {
            get
            {
                DateTime nowD = DateTime.Now;
                double sum = 0;
                TimeSpan ts;
                foreach (Transaction t in _transactions)
                {
                    ts = nowD.Subtract(t.Date);
                    if (t.Sum > 0 & ts.TotalDays < 32)
                    {
                        sum += Convertor.convert(t.Sum, t.Currency, _currency);
                    }
                }

                return sum;
            }
        }
        public bool HasCategory(Category category) => _categories.Contains(category);
        public bool AddCategory(Category category) => _categories.Add(category);
        public bool RemoveCategory(Category category) => _categories.Remove(category);
        public bool AddTransaction(Transaction transaction)
        {
            if (
                _transactions.Contains(transaction) ||
                (transaction.User != null && !transaction.User.HasWallet(this)) ||
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