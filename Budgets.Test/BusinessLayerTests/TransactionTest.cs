using Budgets.BusinessLayer.Entities;
using Xunit;
using Budgets.Common;
using System.Collections.Generic;
using System;

namespace Budgets.Test.BusinessLayerTests
{
    public class TransactionTest
    {
        private User u = new User(Guid.NewGuid(), "Bob", "Johns", "a@a.com");
        private Category ca = new Category("food", "restaurants transactions");
        [Fact]
        public void ValidateValid()
        {
            var c = new Transaction(u, 100, Currency.UAH, ca, "bank transatcion");
            Assert.True(c.IsValid);
        }

        [Fact]
        public void ValidateInvalidDescription()
        {
            var u = new User(Guid.NewGuid(), "Bob", "Johns", "a@a.com");
            var c = new Transaction(u, 100, Currency.UAH, ca, "");
            Assert.False(c.IsValid);
        }
        [Fact]
        public void ValidateValidFilesAttached()
        {
            var u = new User(Guid.NewGuid(), "Bob", "Johns", "a@a.com");
            var c = new Transaction(u, 100, Currency.UAH, ca, "a");
            Assert.True(c.IsValid);
            c.AddFile("attachment.doc");
            c.AddFile("attachment.png");
            Assert.True(c.IsValid);
        }

        [Fact]
        public void ValidateInvalidFilesAttached()
        {
            var u = new User(Guid.NewGuid(), "Bob", "Johns", "a@a.com");
            var c = new Transaction(u, 100, Currency.UAH, ca, "a");
            Assert.True(c.IsValid);
            c.AddFile("attachment._~");
            Assert.False(c.IsValid);
        }


        [Fact]
        public void NewRecordTest()
        {
            var u = new User(Guid.NewGuid(), "Bob", "Johns", "a@a.com");
            var c1 = new Transaction(u, 10, Currency.EUR, ca, "a");

            Assert.True(c1.IsNew);
        }

        [Fact]
        public void ExistingRecordTest()
        {
    
            var c1 = new Transaction(Guid.NewGuid(), Guid.NewGuid(), 10, Currency.EUR, Guid.NewGuid(), "a", new List<string>());

            Assert.False(c1.IsNew);
        }
    }
}
