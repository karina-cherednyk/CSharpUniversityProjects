using Budgets.BusinessLayer.Entities;
using Budgets.Common;
using BudgetsWPF.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugetsStorage.Services
{
    public class AuthUserService: EntityService<AuthUser>
    {
        public static async Task<User> RegisterUserAsync(AuthUser regUser)
        {
            //Thread.Sleep(2000);
            var all = await All;
            var users = all.Values.ToList();
            var dbUser = users.FirstOrDefault(user => user.Login == regUser.Login);

            if (dbUser != null)
                throw new Exception("User already exists");
            if (string.IsNullOrWhiteSpace(regUser.Login) || String.IsNullOrWhiteSpace(regUser.Password) || String.IsNullOrWhiteSpace(regUser.Email))
                throw new ArgumentException("Login, Password or Last Name is Empty");


            var dataUser = regUser.NewUser();
            var defaultWallet = new Wallet(dataUser.Id, "First Wallet", "", 0, Currency.USD);
            dataUser.AddWallet(defaultWallet);

            await Add(regUser);
            await UserService.Add(dataUser);

            return dataUser;
        }

        public static async Task<User> AuthenticateAsync(AuthUser authUser)
        {
            //Thread.Sleep(2000);
            if (String.IsNullOrWhiteSpace(authUser.Login) || String.IsNullOrWhiteSpace(authUser.Password))
                throw new ArgumentException("Login or Password is Empty");
            
            var all = await All;
            var users = all.Values.ToList();

            var dbUser = users.FirstOrDefault(user => user.Login == authUser.Login && user.Password == authUser.Password);
            if (dbUser == null)
                throw new Exception("Wrong Login or Password");

            var dataUser = await UserService.Get(dbUser.Guid);

            return dataUser;
        }
    }
}
