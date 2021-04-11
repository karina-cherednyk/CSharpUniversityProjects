using Budgets.Common;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Budgets.BusinessLayer.Entities
{
	public class User : BaseEntity, ICategorizable
    {
        private string _firstName, _lastName, _email;
        public string LastName { get { return _lastName; } set { _lastName = value; HasChanges = true; } }
        public string FirstName { get { return _firstName; } set { _firstName = value; HasChanges = true; } }

        public string Email { get { return _email; } set { _email = value; HasChanges = true; } }

        [JsonIgnore]
        public string FullName {  get
            {
                return FirstName + " " + LastName;
            } }
        

        [JsonIgnore]
        public HashSet<Category> Categories  => _categories; 
        [JsonIgnore]
        public HashSet<Wallet> Wallets => _wallets; 

        
        private HashSet<Category> _categories;
        private HashSet<Wallet> _wallets;

        [JsonConstructor]
        public User(Guid id,  string firstName, string lastName, string email)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            _categories = new();
            _wallets = new();
        }

        public void RemoveCategory(object category)
        {
            throw new NotImplementedException();
        }

        public bool HasWallet(Wallet w) => _wallets.Contains(w);
        public bool AddCategory(Category category) => _categories.Add(category);
        public bool AddWallet(Wallet wallet) => _wallets.Add(wallet);
        public bool RemoveCategory(Category category) => _categories.Remove(category);
        public bool HasCategory(Category category) => _categories.Contains(category);
        public bool RemoveWallet(Wallet wallet) => _wallets.Remove(wallet);
   

        public override bool Validate()
        {
            bool validEmail =  Validator.ValidateEmail(Email);

            return
                !string.IsNullOrWhiteSpace(FirstName) &
                !string.IsNullOrWhiteSpace(LastName) &
                validEmail;

        }


    }
}