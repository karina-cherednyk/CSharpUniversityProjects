using System.Collections.Generic;

namespace Budgets.Common
{
    public static class CurrencyConvertor
    {
        // dollar to X rate -> USD val = X val / rate
        private static decimal[] currencyRate = new decimal[3];
        static CurrencyConvertor()
        {
            currencyRate[(int)Currency.UAH] = 27.95M;
            currencyRate[(int)Currency.EUR] = 0.83M;
            currencyRate[(int)Currency.USD] = 1M;

        }
        public static decimal convert(decimal sum, Currency from, Currency to)
        {
            if (from == to) return sum;
            decimal sumUSD = sum / currencyRate[(int)from];
            decimal res = sumUSD * currencyRate[(int)to];
            return res;
        }

        public static Currency CurrencyFromString(string cur)
        {
            switch (cur)
            {
                case "USD": return Currency.USD;
                case "EUR": return Currency.EUR;
                case "UAH":
                default: return Currency.UAH;
            }
        }
        public static string CurencyToString(Currency cur)
        {
            switch (cur)
            {
                case Currency.USD: return "USD";
                case Currency.EUR: return "EUR";
                case Currency.UAH: 
                default: return "UAH";
            }
        }
        public static string CurencyToSign(Currency cur)
        {
            switch (cur)
            {
                case Currency.USD: return "$";
                case Currency.EUR: return "€";
                case Currency.UAH:
                default: return "₴";
            }
        }

        public static List<string> AllCurrencies()
        {
            return new List<string>() { "USD", "UAH", "EUR" };
        }

    }
}
