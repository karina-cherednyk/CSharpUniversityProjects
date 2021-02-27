using Budgets.Common;
using System.Collections.Generic;

namespace Budgets.BusinessLayer.Entities
{
	public class User : BaseEntity
    {
        private static int InstanceCount { get; set; }
        private string _lastName;
        private string _firstName;
        private string _email;
        private HashSet<int> _categories;


        User(int id, string firstName, string lastName, string email, HashSet<int> categories)
        {
            Id = id;
            _firstName = firstName;
            _lastName = lastName;
            _email = email;
            _categories = categories;
        }

        User(string firstName, string lastName, string email): 
            this(++InstanceCount, firstName, lastName, email, new HashSet<int>() )
        {
            IsNew = true;
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; HasChanges = true; }
        }


        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; HasChanges = true; }
        }


        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; HasChanges = true; }
        }

        public HashSet<int> UsingCategories
        {
            get { return _categories; }
        }

        public bool addCategory(Category category)
        {
            if (_categories.Add(category.Id))
            {
                HasChanges = true;
                return true;
            }
            return false;
        }
        public bool removeCategory(Category category)
        {
           if(_categories.Remove(category.Id))
            {
                HasChanges = true;
                return true;
            }
            return false;
        }

        public override bool Validate()
        {
            bool validEmail = _email == null || Validator.ValidateEmail(_email);

            return
                string.IsNullOrWhiteSpace(_firstName) &
                string.IsNullOrWhiteSpace(_lastName) &
                validEmail;


        }
    }
}