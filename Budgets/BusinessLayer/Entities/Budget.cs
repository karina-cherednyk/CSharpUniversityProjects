using System.Collections.Generic;
using Budgets.Common;
using System;

namespace Budgets.BusinessLayer.Entities
{
	public class Budget : BaseEntity 
    {
        private static int InstanceCount;


        private int Owner { get;  }
        private string _name;
        private double _initialBalance;
        private string _description;
        private Currency _currency;
        private HashSet<int> _users;
        private HashSet<int> _categories;
        private List<Transaction> _transactions;

        public Budget(int id, int owner,  HashSet<int> users, HashSet<int> categories, List<Transaction> transactions, string name, string description, double initialBalance, Currency currency )
        {
            Id = id;
            Owner = owner;
            _users = users;
            _categories = categories;
            _transactions = transactions;
            _name = name;
            _description = description;
            _initialBalance = initialBalance;
            _currency = currency;
        }

        public Budget(User user, string name, string description, double initialBalance, Currency currency):
            this(++InstanceCount, user.Id, new HashSet<int>(), user.UsingCategories, new List<Transaction>(), name, description, initialBalance, currency)
        {
            _users.Add(user.Id);
            IsNew = true;
        }
        public Currency Currency
        {
            get { return _currency; }
            set {
                _initialBalance = Convertor.convert(_initialBalance, _currency, value);
                _currency = value;
            }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; HasChanges = true; }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; HasChanges = true;  }
        }
        public HashSet<int> Categories
        {
            get { return _categories; }
            set { _categories = value; HasChanges = true; }
        }
        public double Balance
        {
            get {
                double sum = _initialBalance;
                foreach(Transaction t in _transactions)
                {
                     sum += Convertor.convert(t.Sum, t.Currency, _currency);
                }

                return sum;
            }
        }
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

        public HashSet<int> Users()
        {
            return _users;
        }

        public bool AddUser(User user)
        {
            if (_users.Add(user.Id))
            {
                HasChanges = true;
                return true;
            }
            return false;
        }

        public bool RemoveUser(User user)
        {
            if (user.Id == Owner) return false;

            if (_users.Remove(user.Id)) 
            { 
                HasChanges = true;
                return true;
            }
            return false;
        }
        public bool AddCategory(Category category)
        {
            if (_categories.Add(category.Id))
            {
                HasChanges = true;
                return true;
            }
            return false;
        }

        public bool RemoveCategory(Category category)
        {
            if (_categories.Remove(category.Id))
            {
                HasChanges = true;
                return true;
            }
            return false;
        }


        public bool AddTransaction(Transaction transaction)
        {
            int uid = transaction.User;
            int cid = transaction.Category;
            if (
                _transactions.Contains(transaction) ||
                !_users.Contains(uid) ||
                !_categories.Contains(cid)
                )
            {
                return false;
            }
            else
            {
                _transactions.Add(transaction);
                HasChanges = true;
                return true;
            }
        }

        public bool RemoveTransaction(Transaction transaction)
        {
            if (_transactions.Remove(transaction))
            {
                HasChanges = true;
                return true;
            }
            return false;
        }
        public List<Transaction> Transactions()
        {
            return _transactions;
        }

        public override bool Validate()
        {
            return
                !string.IsNullOrWhiteSpace(Name) &
                !string.IsNullOrWhiteSpace(Description) &
                Balance >= 0;

        }
    }
}