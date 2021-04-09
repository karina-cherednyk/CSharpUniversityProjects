using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Budgets.Common;

namespace Budgets.BusinessLayer.Entities
{
    public class Transaction: BaseEntity, ICategorizable
    {
        [JsonIgnore]
        public User User { get; set;  }
        public decimal Sum { get; set; }
        public DateTime Date { get; set; }
        public List<string> Files { get; private set; }
        public string Description { get; set; }
        public Currency Currency { get; set; }

        [JsonIgnore]
        public Category Category { get; set; }

        [JsonConstructor]
        public Transaction(Guid id,  DateTime date, decimal sum, Currency currency, Category category, string description, List<string> files):
        this(id, null, sum, currency, category, description, files )
        {
            Date = date;
        }

        public Transaction(Guid id, User user, decimal sum, Currency cur, Category cat, string desc, List<string> files)
        {
            Id = id;
            User = user;
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
        {}

        public void AddFile(string file) => Files.Add(file);
        public bool HasCategory(Category category) => Category == category;
        public bool AddCategory(Category category)
        {
            Category = category;
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