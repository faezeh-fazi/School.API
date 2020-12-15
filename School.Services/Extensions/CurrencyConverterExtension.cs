using System;
using System.Collections.Generic;
using System.Text;

namespace School.Extensions
{
    public static class CurrencyConverterExtension
    {
        public static decimal Convert(string DefaultCurrency,
            string FromCurrency, 
            string ToCurrency,
            decimal FromRate,
            decimal ToRate,
            decimal Amount)
        {
            decimal Converted = 0;

            if (FromCurrency == ToCurrency)
            {
                return Amount;
            }
            else if (FromCurrency == DefaultCurrency)
            {
                Converted = Amount / ToRate;
                return Converted;
            }
            else if (FromCurrency != DefaultCurrency && ToCurrency == DefaultCurrency)
            {
                Converted = Amount * FromRate;
                return Converted;
            }
            else
            {
                var DefaultAmount = Amount * FromRate;
                Converted = DefaultAmount / ToRate;
                return Converted;
            }
        }
    }
}
