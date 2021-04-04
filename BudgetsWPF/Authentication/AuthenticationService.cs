using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BudgetsWPF.Authentication
{
    class AuthenticationService
    {
        private FileDataStorage<DBUser> _storage = new FileDataStorage<DBUser>();

        public async Task<bool> RegisterUserAsync(DBUser regUser)
        {
            //Thread.Sleep(2000);
            var users = await _storage.GetAllAsync();
            var dbUser = users.FirstOrDefault(user => user.Login == regUser.Login);
            if (dbUser != null)
                throw new Exception("User already exists");
            if (String.IsNullOrWhiteSpace(regUser.Login) || String.IsNullOrWhiteSpace(regUser.Password) || String.IsNullOrWhiteSpace(regUser.Email))
                throw new ArgumentException("Login, Password or Last Name is Empty");
     
            await _storage.AddOrUpdateAsync(regUser);
            return true;
        }

        public async Task<DBUser> AuthenticateAsync(DBUser authUser)
        {
            //Thread.Sleep(2000);
            if (String.IsNullOrWhiteSpace(authUser.Login) || String.IsNullOrWhiteSpace(authUser.Password))
                throw new ArgumentException("Login or Password is Empty");
            var users = await _storage.GetAllAsync();

            var dbUser = users.FirstOrDefault(user => user.Login == authUser.Login && user.Password == authUser.Password);
            if (dbUser == null)
                throw new Exception("Wrong Login or Password");

            return new DBUser(dbUser.Guid, dbUser.Login, dbUser.Email, dbUser.Password);
        }

    }
}
