﻿using Xunit;
using Budgets.BusinessLayer.Entities;
using Budgets.Common;
using System.Collections.Generic;

namespace Budgets.Test.BusinessLayerTests
{
    public class BudgetTest
    {
        private User u = new User("Bob", "Johns", "a@a.com");
        private Category ca = new Category("food", "restaurants transactions");
        public BudgetTest(){
            u.addCategory(ca);
        }

        [Fact]
        public void ValidateValid()
        {
            var c = new Budget(u, "b1", "d1", 0, Currency.UAH);
            Assert.True(c.IsValid);
        }

        [Fact]
        public void ValidateInvalidName()
        {
            var c = new Budget(u, null, "d1", 0, Currency.UAH);
            Assert.False(c.IsValid);
        }
        [Fact]
        public void ValidateInvalidDescription()
        {
            var c = new Budget(u, "b1", "", 0, Currency.UAH);
            Assert.False(c.IsValid);
        }

        [Fact]
        public void ValidateInvalidInitialBudget()
        {
            var c = new Budget(u, "b1", "d1", -10, Currency.UAH);
            Assert.False(c.IsValid);
        }
        [Fact]
        public void CheckMonthProfit()
        {
            var c = new Budget(u, "b1", "d1", 0, Currency.EUR);
            var profit = 10.0;
            var t1 = new Transaction(u, profit, Currency.EUR, ca, "a");
            var t2 = new Transaction(u, -100, Currency.EUR, ca, "a");
            var t3 = new Transaction(u, 1000, Currency.EUR, ca, "a");
            t3.Date = new System.DateTime(2000, 10, 20);

            c.AddTransaction(t1);
            c.AddTransaction(t2);
            c.AddTransaction(t3);

            Assert.Equal(c.MonthProfit, profit, 2);
        }
        [Fact]
        public void CheckMonthLoss()
        {
            var c = new Budget(u, "b1", "d1", 0, Currency.EUR);
            var loss = 10.0;
            var t1 = new Transaction(u, -loss, Currency.EUR, ca, "a");
            var t2 = new Transaction(u, 100, Currency.EUR, ca, "a");
            var t3 = new Transaction(u, -1000, Currency.EUR, ca, "a");
            t3.Date = new System.DateTime(2000, 10, 20);

            c.AddTransaction(t1);
            c.AddTransaction(t2);
            c.AddTransaction(t3);

            Assert.Equal(c.MonthLoss, loss, 2);
        }


        [Fact]
        public void CheckBalance()
        {
            var c = new Budget(u, "b1", "d1", 0, Currency.EUR);
            var t1 = new Transaction(u, 10, Currency.EUR, ca, "a");
            var t2 = new Transaction(u, 10, Currency.EUR, ca, "a");
            Assert.True(c.AddTransaction(t1));
            Assert.True(c.AddTransaction(t2));
            Assert.Equal(20, c.Balance);
        }
        [Fact]
        public void CheckCurrencyConversion()
        {
            var initEu = 10;
            var addUah = 10;
            var resEuro = initEu + Convertor.convert(addUah, Currency.UAH, Currency.EUR);

            var c = new Budget(u, "b1", "d1", initEu, Currency.EUR);
            var t1 = new Transaction(u, 10, Currency.UAH, ca, "a");
            c.AddTransaction(t1);

            Assert.Equal(resEuro, c.Balance, 2);
        }
        [Fact]
        public void AddTransactionOfNotAllowedCategory()
        {
            var c = new Budget(u, "b1", "d1", 0, Currency.EUR);
            var  cb  = new Category("food1", "restaurants transactions");
            var t1 = new Transaction(u, 10, Currency.EUR, cb, "a");
            Assert.False(c.AddTransaction(t1));
        }
        [Fact]
        public void AllowCategoryAndAddTransaction()
        {
            var c = new Budget(u, "b1", "d1", 0, Currency.EUR);
            var cb = new Category("food1", "restaurants transactions");
            var t1 = new Transaction(u, 10, Currency.EUR, cb, "a");
            c.AddCategory(cb);
            Assert.True(c.AddTransaction(t1));
        }
        [Fact]
        public void AddTransactionFromNotAllowedUser()
        {
            var c = new Budget(u, "b1", "d1", 0, Currency.EUR);
            var u1 = new User("Bob", "Johns", "a@a.com");
            var t1 = new Transaction(u1, 10, Currency.EUR, ca, "a");
            Assert.False(c.AddTransaction(t1));
        }
        [Fact]
        public void AllowUserAndAddTransaction()
        {
            var c = new Budget(u, "b1", "d1", 0, Currency.EUR);
            var u1 = new User("Bob", "Johns", "a@a.com");
            var t1 = new Transaction(u1, 10, Currency.EUR, ca, "a");
            c.AddUser(u1);
            Assert.True(c.AddTransaction(t1));
        }

        [Fact]
        public void AddTransactionTwice()
        {
            var c = new Budget(u, "b1", "d1", 0, Currency.EUR);
            var t1 = new Transaction(u, 10, Currency.EUR, ca, "a");
            Assert.True(c.AddTransaction(t1));
            Assert.False(c.AddTransaction(t1));
        }



        [Fact]
        public void NewRecordTest()
        {
            var c = new Budget(u, "b1", "d1", 0, Currency.UAH);

            Assert.True(c.IsNew);
        }

        [Fact]
        public void ExistingRecordTest()
        {
            var c = new Budget(1, 1, 
                new HashSet<int>(), new HashSet<int>(), 
                new List<Transaction>(),"a", 
                "b", 0, Currency.UAH);


            Assert.False(c.IsNew);
        }

        [Fact]
        public void CounterTest()
        {
            var c1 = new Budget(u, "b1", "d1", 0, Currency.UAH);
            var c2 = new Budget(u, "b1", "d1", 0, Currency.UAH);
            var c3 = new Budget(u, "b1", "d1", 0, Currency.UAH);

            Assert.Equal(c2.Id, c1.Id + 1);
            Assert.Equal(c3.Id, c2.Id + 1);
        }
    }
}