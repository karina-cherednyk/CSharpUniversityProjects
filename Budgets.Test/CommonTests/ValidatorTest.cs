using Xunit;
using Budgets.Common;
namespace Budgets.Test.CommonTests
{
    public class ValidatorTest
    {
        [Fact]
        public void CheckValidFileValidation()
        {
            string[] validfiles = { "a.txt", "a.png", "899_a.jpg" };

            var a = Validator.FileRegex;
            foreach(string file in validfiles)
            {
                Assert.True(Validator.ValidateFile(file));
            }
        }

        [Fact]
        public void CheckInvalidFileValidation()
        {
            string[] invalidfiles = { ".a.txt", "a.Png", "a\\899_a.jpg", "o.psd" };
            foreach (string file in invalidfiles)
            {
                Assert.False(Validator.ValidateFile(file));
            }
        }
        [Fact]
        public void CheckValidColorValidation()
        {
            string[] validcolors = { "#FFFFFF", "#128757" };
            foreach (string color in validcolors)
            {
                Assert.True(Validator.ValidateColor(color));
            }
        }
        [Fact]
        public void CheckInvalidColorValidation()
        {
            string[] validcolors = { "FFFFFF", "white" };
            foreach (string color in validcolors)
            {
                Assert.False(Validator.ValidateColor(color));
            }
        }
        [Fact]
        public void CheckValidEmailValidation()
        {
            string[] validemails = { "aa@gmail.com", "rkayshavoq@xcy1.xyz" };
            foreach (string email in validemails)
            {
                Assert.True(Validator.ValidateEmail(email));
            }
        }
        [Fact]
        public void CheckInvalidEmailValidation()
        {
            string[] invalidemails = { "aagmail.com", "not email" };
            foreach (string email in invalidemails)
            {
                Assert.False(Validator.ValidateEmail(email));
            }
        }

        [Fact]
        public void CheckValidIconValidation()
        {
            string validicon = "icon.ico";
            Assert.True(Validator.ValidateIcon(validicon));
        }
        [Fact]
        public void CheckInvalidIconValidation()
        {
            string invalidicon = "icon.png";
            Assert.False(Validator.ValidateIcon(invalidicon));
        }
    }
}
