using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Budgets.BusinessLayer.Entities
{
    public class AuthUser: IStorable
    {
        public Guid   Guid      { get; }
        public string Email     { get; set; }
        public string Login     { get; set; }
        public string Password  { get; set; }


        [JsonConstructor]
        public AuthUser(Guid guid, string login, string email, string password)
        {
            this.Login = login;
            this.Email = email;
            this.Password = password;
            this.Guid = guid;
        }
        public AuthUser(string login, string email, string password)
        {
            this.Login = login;
            this.Email = email;
            this.Password = password;
            this.Guid = Guid.NewGuid();
        }
        public AuthUser()
        {
            this.Guid = Guid.NewGuid();
        }

        public User NewUser()
        {
            return new User(Guid, "UserName", "UserLastName", Email);
        }

    }
}
