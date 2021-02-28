using Xunit;
using Budgets.Common;

namespace Budgets.Test.CommonTests
{
    public class ConvertorTest
    {
        [Fact]
        public void ConvertAndBack()
        {
            Currency[] currs = { Currency.UAH, Currency.USD, Currency.EUR };

            double valFrom = 10.50;
            double valTo, valBack;

            foreach (Currency from in currs)
            {
                foreach (Currency to in currs)
                {
                    valTo = Convertor.convert(valFrom, from, to);
                    valBack = Convertor.convert(valTo, to, from);
                    Assert.Equal(valBack, valFrom, 2);
                }
            }
        }
    }
}
