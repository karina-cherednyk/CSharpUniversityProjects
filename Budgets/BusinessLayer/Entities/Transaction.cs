using System;
using System.Collections.Generic;
using Budgets.Common;

namespace Budgets.BusinessLayer.Entities
{
    public class Transaction: BaseEntity
    {
        private static int InstanceCount { get; set; }
        private Currency _currency;
        private int _category;
        private string _description;
        private DateTime _date;
        private double _sum;
        private List<string> _files;

        public int User { get; }

        public Transaction(int id, int user, double sum, Currency cur, int cat, string desc, List<string> files)
        {
            Id = id;
            User = user;
            _sum = sum;
            _currency = cur;
            _category = cat;
            _description = desc;
            _files = files;
            _date = DateTime.Now;
        }
        public Transaction(User user, double sum, Currency cur, Category cat, string desc) :
            this(++InstanceCount, user.Id, sum, cur, cat.Id, desc, new List<string>())
        {
            IsNew = true;
        }

        public double Sum
        {
            get { return _sum; }
            set { _sum = value; HasChanges = true; }
        }


        public DateTime Date
        {
            get { return _date; }
            set { _date = value; HasChanges = true; }
        }
        public List<string> Files
        {
            get { return _files; }
        }
        public void AddFile(string file)
        {
            _files.Add(file);
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; HasChanges = true; }
        }


        public Currency Currency
        {
            get { return _currency; }
            set { _currency = value; HasChanges = true; }
        }

        public int Category
        {
            get { return _category; }
            set { _category = value; HasChanges = true; }
        }

        public override bool Validate()
        {
            bool validFiles = true;
            foreach (string file in _files){
                validFiles &= Validator.ValidateFile(file);
            }
            
            return
                !string.IsNullOrWhiteSpace(_description) &
                _sum != 0 &
                validFiles;
        }

    }
}