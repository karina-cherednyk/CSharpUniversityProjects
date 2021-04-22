using System;
using System.Collections.Generic;
using Budgets.BusinessLayer.Entities;
using Xunit;

namespace Budgets.Test.BusinessLayerTests
{
    public class UserTest
    {
        [Fact]
        public void ValidateValid()
        {
            var c = new User(Guid.NewGuid(), "Bob", "Johns", "a@a.com");
            Assert.True(c.IsValid);
        }

        [Fact]
        public void ValidateInvalidName()
        {
            var c = new User(Guid.NewGuid(), null, "Johns", "a@a.com");
            Assert.False(c.IsValid);
        }
        [Fact]
        public void ValidateInvalidSurname()
        {
            var c = new User(Guid.NewGuid(), "Bob", "", "a@a.com");
            Assert.False(c.IsValid);
        }

        [Fact]
        public void ValidateInvalidEmal()
        {
            var c = new User(Guid.NewGuid(), "Bob", "Johns", "aa.com");
            Assert.False(c.IsValid);
        }



        [Fact]
        public void ExistingRecordTest()
        {
            var c = new User(Guid.NewGuid(),  "Bob", "Johns", "a@a.com");

            Assert.False(c.IsNew);
        }

        [Fact]
        public void AddNewCategory()
        {
            var c = new User(Guid.NewGuid(), "Bob", "Johns", "a@a.com");
            var ca = new Category("food", "restaurants transactiosn");
            var cb = new Category("food", "restaurants transactiosn");

            Assert.True(c.AddCategory(ca));
            Assert.True(c.AddCategory(cb));
        }

        [Fact]
        public void AddExistingCategory()
        {
            var c = new User(Guid.NewGuid(), "Bob", "Johns", "a@a.com");
            var ca = new Category("food", "restaurants transactiosn");

            Assert.True(c.AddCategory(ca));
            Assert.False(c.AddCategory(ca));
        }
    }
}
