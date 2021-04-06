using Xunit;
using Budgets.BusinessLayer.Entities;
using System;

namespace Budgets.Test.BusinessLayerTests
{
    public class CategoryTest
    {
        [Fact]
        public void ValidateValid()
        {
            var c = new Category("food", "restaurants transactions");
            Assert.True(c.IsValid);
        }

        [Fact]
        public void ValidateInvalidColor()
        {
            var c = new Category("food", "restaurants transactions");
            Assert.True(c.IsValid);
            c.Color = "blue"; // must be #XXXXXX
            Assert.False(c.IsValid);
        }
        [Fact]
        public void ValidateInvalidIcon()
        {
            var c = new Category("food", "restaurants transactions");
            Assert.True(c.IsValid);
            c.Icon = "blue"; 
            Assert.False(c.IsValid);
        }
        [Fact]
        public void ValidateNoName()
        {
            var c = new Category("", "restaurants transactiosn");
            Assert.False(c.IsValid);
        }
        [Fact]
        public void ValidateNoDescription()
        {
            var c = new Category("a", "descr");
            c.Description = null;
            Assert.False(c.IsValid);
        }

        [Fact]
        public void HasChanges()
        {
            var c = new Category("food", "restaurants transactions");
            c.Description = "shops transactions";
            Assert.True(c.HasChanges);
        }

        [Fact]
        public void NewRecordTest()
        {
            var c1 = new Category("food", "restaurants transactions");

            Assert.True(c1.IsNew);
        }

        [Fact]
        public void ExistingRecordTest()
        {
            var c1 = new Category(Guid.NewGuid(), "food", "restaurants transactions", null, null);

            Assert.False(c1.IsNew);
        }
    }
}
