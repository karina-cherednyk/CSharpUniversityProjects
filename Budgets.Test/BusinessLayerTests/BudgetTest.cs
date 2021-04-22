using Xunit;
using Budgets.BusinessLayer.Entities;
using Budgets.Common;
using System;

namespace Budgets.Test.BusinessLayerTests
{
    public class BudgetTest
    {
        private User u = new User(Guid.NewGuid(), "Bob", "Johns", "a@a.com");
        private Category ca = new Category("food", "restaurants transactions");
        public BudgetTest(){
            u.AddCategory(ca);
        }

        [Fact]
        public void ValidateValid()
        {
            var c = new Wallet(u.Id, "b1", "d1", 0, Currency.UAH);
            Assert.True(c.IsValid);
        }

        [Fact]
        public void ValidateInvalidName()
        {
            var c = new Wallet(u.Id, null, "d1", 0, Currency.UAH);
            Assert.False(c.IsValid);
        }
        [Fact]
        public void ValidateInvalidDescription()
        {
            var c = new Wallet(u.Id, "b1", "", 0, Currency.UAH);
            Assert.False(c.IsValid);
        }

        [Fact]
        public void ValidateInvalidInitialBudget()
        {
            var c = new Wallet(u.Id, "b1", "d1", -10, Currency.UAH);
            Assert.False(c.IsValid);
        }
        [Fact]
        public void CheckMonthProfit()
        {
            var u = new User(Guid.NewGuid(), "Bob", "Johns", "a@a.com");
            var w = new Wallet(u.Id, "b1", "d1", 0, Currency.EUR);
            u.AddWallet(w);
            w.AddCategory(ca);
            decimal profit = 10.0M;
            var t1 = new Transaction(u, profit, Currency.EUR, ca, "a");
            var t2 = new Transaction(u, 100, Currency.EUR, ca, "a");
            var t3 = new Transaction(u, -1000, Currency.EUR, ca, "a");
            t2.Date = new System.DateTime(2000, 10, 20);

            w.AddTransaction(t1);
            w.AddTransaction(t2);
            w.AddTransaction(t3);

            Assert.Equal(w.MonthProfit, profit, 2);
        }
        [Fact]
        public void CheckMonthLoss()
        {
            var u = new User(Guid.NewGuid(), "Bob", "Johns", "a@a.com");
            var w = new Wallet(u.Id, "b1", "d1", 0, Currency.EUR);
            u.AddWallet(w);
            w.AddCategory(ca);
            decimal loss = 10.0M;
            var t1 = new Transaction(u, -loss, Currency.EUR, ca, "a");
            var t2 = new Transaction(u, 100, Currency.EUR, ca, "a");
            var t3 = new Transaction(u, -1000, Currency.EUR, ca, "a");
            t3.Date = new System.DateTime(2000, 10, 20);

            w.AddTransaction(t1);
            w.AddTransaction(t2);
            w.AddTransaction(t3);

            Assert.Equal(w.MonthLoss, loss, 2);
        }


        [Fact]
        public void CheckBalance()
        {
            var u = new User(Guid.NewGuid(), "Bob", "Johns", "a@a.com");
            
            var w = new Wallet(u.Id, "b1", "d1", 0, Currency.EUR);
            u.AddWallet(w);
            w.AddCategory(ca);
            var t1 = new Transaction(u, 10, Currency.EUR, ca, "a");
            var t2 = new Transaction(u, 10, Currency.EUR, ca, "a");
            Assert.True(w.AddTransaction(t1));
            Assert.True(w.AddTransaction(t2));
            Assert.Equal(20, w.Balance);
        }
        [Fact]
        public void CheckCurrencyConversion()
        {
            var initEu = 10;
            var addUah = 10;
            var resEuro = initEu + CurrencyConvertor.convert(addUah, Currency.UAH, Currency.EUR);
            
            var u = new User(Guid.NewGuid(), "Bob", "Johns", "a@a.com");
            var w = new Wallet(u.Id, "b1", "d1", initEu, Currency.EUR);
            u.AddWallet(w);
            w.AddCategory(ca);
            var t1 = new Transaction(u, 10, Currency.UAH, ca, "a");
            w.AddTransaction(t1);

            Assert.Equal(resEuro, w.Balance, 2);
        }
        [Fact]
        public void AddTransactionOfNotAllowedCategory()
        {
            var c = new Wallet(u.Id, "b1", "d1", 0, Currency.EUR);
            var  cb  = new Category("food1", "restaurants transactions");
            var t1 = new Transaction(u, 10, Currency.EUR, cb, "a");
            Assert.False(c.AddTransaction(t1));
        }
        [Fact]
        public void AllowCategoryAndAddTransaction()
        {
            var u = new User(Guid.NewGuid(), "Bob", "Johns", "a@a.com");
            var w = new Wallet(u.Id, "b1", "d1", 0, Currency.EUR);
            var c = new Category("food1", "restaurants transactions");
            var t1 = new Transaction(u, 10, Currency.EUR, c, "a");
            u.AddWallet(w);
            w.AddCategory(c);
            Assert.True(w.AddTransaction(t1));
        }
        [Fact]
        public void AddTransactionFromNotAllowedUser()
        {
            var c = new Wallet(u.Id, "b1", "d1", 0, Currency.EUR);
            var u1 = new User(Guid.NewGuid(), "Bob", "Johns", "a@a.com");
            var t1 = new Transaction(u1, 10, Currency.EUR, ca, "a");
            Assert.False(c.AddTransaction(t1));
        }


        [Fact]
        public void AddTransactionTwice()
        {
            var w = new Wallet(u.Id, "b1", "d1", 0, Currency.EUR);
            w.AddCategory(ca);
            u.AddWallet(w);
            var t1 = new Transaction(u, 10, Currency.EUR, ca, "a");
            Assert.True(w.AddTransaction(t1));
            Assert.False(w.AddTransaction(t1));
        }



        [Fact]
        public void NewRecordTest()
        {
            var c = new Wallet(u.Id, "b1", "d1", 0, Currency.UAH);

            Assert.True(c.IsNew);
        }

        [Fact]
        public void ExistingRecordTest()
        {
            var c = new Wallet(Guid.NewGuid(), Guid.NewGuid(),"a", 
                "b", 0, Currency.UAH, 0, 0, 0);


            Assert.False(c.IsNew);
        }


    }
}
