using Xunit;
using Budgets.BusinessLayer.Entities;

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
        public void CounterTest()
        {
            var c1 = new Category("food", "restaurants transactions");
            var c2 = new Category("shopping", "d1");
            var c3 = new Category("food", "d2");

            Assert.Equal(c2.Id, c1.Id + 1);
            Assert.Equal(c3.Id, c2.Id + 1);
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
            var c1 = new Category(1, "food", "restaurants transactions", null, null);

            Assert.False(c1.IsNew);
        }
    }
}
