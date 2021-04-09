using Budgets.BusinessLayer.Entities;
using Budgets.Common;
using BugetsStorage.Services;
using System;
using System.Linq;
using Xunit;

namespace BudgetsStorage.Test
{
    public class UnitTest1
    {
        [Fact]
        public async void Test1()
        {
            UserService _userService = new();


            AuthUser userAuth = new("john", "john@gmail.com", "pass");
            var user = userAuth.NewUser();
            user.FirstName = "John";
            user.LastName = "Johnson";

            await UserService.Add(user);
            var user2 = await UserService.Get(user.Id);

        }
        [Fact]
        public async void Test2()
        {
  
            AuthUser userAuth = new("john", "john@gmail.com", "pass");
            var user = userAuth.NewUser();
            user.FirstName = "John";
            user.LastName = "Johnson";

            user.AddWallet(new Wallet(user.Id, "Wallet", "Desc", 0, Currency.UAH));

            await UserService.Add(user);
            var user2 = await UserService.Get(user.Id);
        }

        [Fact]
        public async void Test3()
        {


            AuthUser userAuth = new("john", "john@gmail.com", "pass");
            var user = userAuth.NewUser();
            user.FirstName = "John";
            user.LastName = "Johnson";

            var cat = new Category("a", "b");
            user.AddCategory(cat);

            var wall = new Wallet(user.Id, "Wallet", "Desc", 0, Currency.UAH);
           
            var trans = new Transaction(user, 10, Currency.EUR, cat, "desc");

            user.AddWallet(wall);
            wall.AddCategory(cat);
            wall.AddTransaction(trans);

            await UserService.Add(user);
            await CategoryService.Add(cat);

            var user2 = await UserService.Get(user.Id);
           

        }
        [Fact]
        public async void Test4()
        {
            AuthUser userAuth = new("john", "john@gmail.com", "pass");
            await AuthUserService.RegisterUserAsync(userAuth);
            var user2 = await AuthUserService.AuthenticateAsync(userAuth);
            var a = 3;
        }
    }
}