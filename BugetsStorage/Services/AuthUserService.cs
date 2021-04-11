using Budgets.BusinessLayer.Entities;
using Budgets.Common;
using System;
using System.Linq;

using System.Threading.Tasks;

namespace BudgetsStorage.Services
{
    public class AuthUserService: EntityService<SignInUser>
    {
        public static async Task<User> RegisterUserAsync(SignUpUser signUpUser)
        {
            //Thread.Sleep(2000);
            var signInUser = signUpUser.NewSignInUser();
            signInUser.EncryptPassword();

            var all = await All;
            var users = all.Values.ToList();
            var dbUser = users.FirstOrDefault(user => signInUser.Equals(user));

            if (dbUser != null)
                throw new Exception("User already exists");

            if (string.IsNullOrWhiteSpace(signInUser.Login) || string.IsNullOrWhiteSpace(signInUser.Password) || string.IsNullOrWhiteSpace(signInUser.Email))
                throw new ArgumentException("Login, Password or Last Name is Empty");


            var dataUser = new User(signInUser.Guid, signUpUser.FirstName, signUpUser.LastName, signUpUser.Email);
            var defaultWallet = new Wallet(dataUser.Id, "First Wallet", "Firts Description", 0, Currency.USD);
            dataUser.AddWallet(defaultWallet);

            await Add(signInUser);
            await UserService.Add(dataUser);

            return dataUser;
        }

        public static async Task<User> AuthenticateAsync(SignInUser signInUser)
        {
            //Thread.Sleep(2000);
            if (string.IsNullOrWhiteSpace(signInUser.Login) || string.IsNullOrWhiteSpace(signInUser.Password))
                throw new ArgumentException("Login or Password is Empty");

            signInUser.EncryptPassword();

            var all = await All;
            var users = all.Values.ToList();

            var dbUser = users.FirstOrDefault(user => signInUser.Equals(user));
            if (dbUser == null)
                throw new Exception("Wrong Login or Password");

            var dataUser = await UserService.Get(dbUser.Guid);

            return dataUser;
        }
    }
}
