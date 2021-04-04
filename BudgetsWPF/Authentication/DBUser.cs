using System;
using System.Text.Json.Serialization;

namespace BudgetsWPF.Authentication
{
    public class DBUser: IStorable
    {
        public Guid   Guid      { get; }
        public string Email     { get; set; }
        public string Login     { get; set; }
        public string Password  { get; set; }


        [JsonConstructor]
        public DBUser(Guid guid, string login, string email, string password)
        {
            this.Login = login;
            this.Email = email;
            this.Password = password;
            this.Guid = guid;
        }
        public DBUser(string login, string email, string password)
        {
            this.Login = login;
            this.Email = email;
            this.Password = password;
            this.Guid = Guid.NewGuid();
        }
        public DBUser()
        {
            this.Guid = Guid.NewGuid();
        }

    }
}
