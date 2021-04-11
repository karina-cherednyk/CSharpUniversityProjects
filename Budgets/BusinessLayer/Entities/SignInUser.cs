using Budgets.Common;
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
            Login = login;
            Email = email;
            Password = password;
            Guid = guid;
        }
        public SignInUser(string login, string email, string password)
            :this(Guid.NewGuid(), login, email, password)
        {}
        public SignInUser()
        {
            this.Guid = Guid.NewGuid();
        }

        public void EncryptPassword()
        {
            Password = Encryptor.Encrypt(Password);
        }
        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                SignInUser u = (SignInUser)obj;
                return u.Login == Login && u.Password == Password;
            }
        }

        [JsonIgnore]
        public bool HasChanges { get; set; }

    }
}
