using System;
using System.Globalization;

namespace PlumPack.Infrastructure
{
    public static class CurrencyExtensions
    {
        // Only USD.
        private static CultureInfo _currencyCulture = new CultureInfo("en-US");
        
        public static decimal TrimMoney(this decimal value)
        {
            var factor = (int)Math.Pow(10, _currencyCulture.NumberFormat.CurrencyDecimalDigits);
            return Math.Truncate(factor * value) / factor;
        }

        public static string ToCurrency(this decimal value)
        {
            return value.ToString("C", _currencyCulture);
        }
    }
}