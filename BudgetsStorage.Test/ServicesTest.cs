using Budgets.BusinessLayer.Entities;
using Budgets.Common;
using BudgetsStorage.Services;
using System;
using System.Linq;
using Xunit;

namespace BudgetsStorage.Test
{
    public class ServicesTest
    {
        [Fact]
        public async void NoKeyAbsentException()
        {
            var user = new User(Guid.NewGuid(), "John", "Johnson", "john");

            await UserService.Add(user);
            await UserService.Get(user.Id);


        }
        [Fact]
        public async void WalletIsLoadedWithUser()
        {

            var user = new User(Guid.NewGuid(), "John", "Johnson", "john");
            var w = new Wallet(user.Id, "Wallet", "Desc", 0, Currency.UAH);
            user.AddWallet(w);

            await UserService.Add(user);
            await WalletService.Add(w);
            await RelationService<User, Wallet>.AddConnection(user, w);

            var u = await UserService.Get(user.Id);
            Assert.Equal(w, u.Wallets.First());
        }

        [Fact]
        public async void TransactionsCanBeLoaded()
        {


            var user = new User(Guid.NewGuid(), "John", "Johnson", "john");

            var cat = new Category("a", "b");
            user.AddCategory(cat);

            var wall = new Wallet(user.Id, "Wallet", "Desc", 0, Currency.UAH);
           
            var tran = new Transaction(user, 10, Currency.EUR, cat, "desc");

            user.AddWallet(wall);
            wall.AddCategory(cat);
            wall.AddTransaction(tran);

            await UserService.Add(user);
            await CategoryService.Add(cat);
            await WalletService.Add(wall);
            await TransactionService.Add(tran);
            await RelationService<User, Wallet>.AddConnection(user, wall);
            await RelationService<Wallet, Transaction>.AddConnection(wall, tran);


            var user2 = await UserService.Get(user.Id);
            var wall2 = user2.Wallets.First();
            Assert.Equal(wall, wall2);
            await TransactionService.FillTransactions(wall2);
            var tran2 = wall2.Transactions[0];
            Assert.Equal(tran, tran2);
           

        }

    }
}
