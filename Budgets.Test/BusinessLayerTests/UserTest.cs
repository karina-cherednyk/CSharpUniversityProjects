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
            var c = new User("Bob", "Johns", "a@a.com");
            Assert.True(c.IsValid);
        }

        [Fact]
        public void ValidateInvalidName()
        {
            var c = new User(null, "Johns", "a@a.com");
            Assert.False(c.IsValid);
        }
        [Fact]
        public void ValidateInvalidSurname()
        {
            var c = new User("Bob", "", "a@a.com");
            Assert.False(c.IsValid);
        }

        [Fact]
        public void ValidateInvalidEmal()
        {
            var c = new User("Bob", "Johns", "aa.com");
            Assert.False(c.IsValid);
        }

        [Fact]
        public void CounterTest()
        {
            var c1 = new User("Bob", "Johns", "a@a.com");
            var c2 = new User("Bob", "Johns", "a@a.com");
            var c3 = new User("Bob", "Johns", "a@a.com");

            Assert.Equal(c2.Id, c1.Id + 1);
            Assert.Equal(c3.Id, c2.Id + 1);
        }

        [Fact]
        public void NewRecordTest()
        {
            var c = new User("Bob", "Johns", "a@a.com");

            Assert.True(c.IsNew);
        }

        [Fact]
        public void ExistingRecordTest()
        {
            var uId = 1;
            var c = new User(uId, "Bob", "Johns", "a@a.com", new HashSet<int>());

            Assert.False(c.IsNew);
        }

        [Fact]
        public void AddNewCategory()
        {
            var c = new User("Bob", "Johns", "a@a.com");
            var ca = new Category("food", "restaurants transactiosn");
            var cb = new Category("food", "restaurants transactiosn");

            Assert.True(c.addCategory(ca));
            Assert.True(c.addCategory(cb));
        }

        [Fact]
        public void AddExistingCategory()
        {
            var c = new User("Bob", "Johns", "a@a.com");
            var ca = new Category("food", "restaurants transactiosn");

            Assert.True(c.addCategory(ca));
            Assert.False(c.addCategory(ca));
        }
    }
}
