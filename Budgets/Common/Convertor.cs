namespace Budgets.Common
{
    public static class Convertor
    {
        // dollar to X rate -> USD val = X val / rate
        private static double[] currencyRate = new double[3];
        static Convertor()
        {
            currencyRate[(int)Currency.UAH] = 27.95;
            currencyRate[(int)Currency.EUR] = 0.83;
            currencyRate[(int)Currency.USD] = 1;

        }
        public static double convert(double sum, Currency from, Currency to)
        {
            if (from == to) return sum;
            double sumUSD = sum / currencyRate[(int)from];
            double res = sumUSD * currencyRate[(int)to];
            return res;
        }
    }
}
