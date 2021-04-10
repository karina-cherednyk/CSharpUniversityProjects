using System;
using System.Text.Json.Serialization;

namespace Budgets.BusinessLayer.Entities
{
    public class SignInUser: IStorable
    {
        public Guid   Guid      { get; }
        public string Email     { get; set; }
        public string Login     { get; set; }
        public string Password  { get; set; }


        [JsonConstructor]
        public SignInUser(Guid guid, string login, string email, string password)
        {
            this.Login = login;
            this.Email = email;
            this.Password = password;
            this.Guid = guid;
        }
        public SignInUser(string login, string email, string password)
        {
            this.Login = login;
            this.Email = email;
            this.Password = password;
            this.Guid = Guid.NewGuid();
        }
        public SignInUser()
        {
            this.Guid = Guid.NewGuid();
        }


    }
}
