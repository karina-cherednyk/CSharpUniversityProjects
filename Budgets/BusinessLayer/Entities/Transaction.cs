using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Budgets.Common;

namespace Budgets.BusinessLayer.Entities
{
    public class Transaction : BaseEntity, ICategorizable
    {
        private decimal _sum;
        private DateTime _date;
        private string _description;
        private Currency _currency;
        private Guid _ownerId;

        [JsonIgnore]
        public User Owner { get; set; }

        public Guid OwnerId { 
            get { 
                if (Owner == null) return _ownerId; 
                else return Owner.Id; 
            } 
            set
            {
                _ownerId = value;
            }
        }

        public decimal Sum { get { return _sum; } set { _sum = value; HasChanges = true; } }
        public DateTime Date { get { return _date; } set { _date = value; } }
        public List<string> Files { get; }
        public string Description { get { return _description;  } set { _description = value; HasChanges = true; } }
        public Currency Currency { get { return _currency;  } set { _currency = value; HasChanges = true; } }

        [JsonIgnore]
        public Category Category { get; set; }

        [JsonConstructor]
        public Transaction(Guid id, DateTime date, decimal sum, Currency currency, Category category, string description, List<string> files) :
        this(id, null, sum, currency, category, description, files)
        {
            Date = date;
        }

        public Transaction(Guid id, User user, decimal sum, Currency cur, Category cat, string desc, List<string> files)
        {
            Id = id;
            Owner = user;
            Sum = sum;
            Currency = cur;
            Category = cat;
            Description = desc;
            Files = files;
            Date = DateTime.Now;
            Files = new();
        }
        public Transaction(User user, decimal sum, Currency cur, Category cat, string desc) :
            this(Guid.NewGuid(), user, sum, cur, cat, desc, new List<string>())
        {
            IsNew = true;
        }

        public void AddFile(string file)  { Files.Add(file); HasChanges = true; }
        public bool HasCategory(Category category) => Category == category;
        public bool AddCategory(Category category)
        {
            Category = category;
            HasChanges = true;
            return true;
        }
        public override bool Validate()
        {
            bool validFiles = true;
            foreach (string file in Files)
            {
                validFiles &= Validator.ValidateFile(file);
            }

            return
                !string.IsNullOrWhiteSpace(Description) &
                Sum != 0 &
                validFiles;
        }
    }
}