using System;
using System.Collections.Generic;
using Budgets.Common;

namespace Budgets.BusinessLayer.Entities
{
    public class Transaction: BaseEntity
    {
        private Currency _currency;
        private int _category;
        private string _description;
        private DateTime _date;
        private double _sum;
        private List<string> _files;

        public int User { get; }

        public Transaction(int user, int sum, Currency cur, int cat, string desc, List<String> files)
        {
            User = user;
            _sum = sum;
            _currency = cur;
            _category = cat;
            _description = desc;
            _files = files;
            _date = DateTime.Now;
        }
        public Transaction(int user, int sum, Currency cur, int cat, string desc) :
            this(user, sum, cur, cat, desc, null)
        { }

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
            set { _files = value; }
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
            if(_files != null)
            {
                foreach (string file in _files){
                    validFiles &= Validator.ValidateFile(file);
                }
            }

            return
                !string.IsNullOrWhiteSpace(_description) &
                validFiles;
        }

    }
}