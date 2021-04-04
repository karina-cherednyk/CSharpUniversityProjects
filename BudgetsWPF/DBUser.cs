using System;
using System.Text.Json.Serialization;

namespace BudgetsWPF
{
    public class DBUser: IStorable
    {
        public Guid Guid { get; }
        public string Email { get; }
        public string Login { get; }
        public string Password { get; }


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

    }
}
