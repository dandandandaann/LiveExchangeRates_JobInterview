using System.Collections.Immutable;
using Microsoft.Extensions.Caching.Memory;

namespace IonicCurrencyExchange;

public class ExchangeRatesCache(IMemoryCache cache)
{
    // TODO: move this somewhere else? Or populate on the go?
    public ImmutableArray<string> AvailableCurrencies =
    [
        "ADA", "AED", "AFN", "ALL", "AMD", "ANG", "AOA", "ARB", "ARS", "AUD", "AWG", "AZN", "BAM", "BBD", "BDT", "BGN",
        "BHD", "BIF", "BMD", "BNB", "BND", "BOB", "BRL", "BSD", "BTC", "BTN", "BWP", "BYN", "BYR", "BZD", "CAD", "CDF",
        "CHF", "CLF", "CLP", "CNY", "COP", "CRC", "CUC", "CUP", "CVE", "CZK", "DAI", "DJF", "DKK", "DOP", "DOT", "DZD",
        "EGP", "ERN", "ETB", "ETH", "EUR", "FJD", "FKP", "GBP", "GEL", "GGP", "GHS", "GIP", "GMD", "GNF", "GTQ", "GYD",
        "HKD", "HNL", "HRK", "HTG", "HUF", "IDR", "ILS", "IMP", "INR", "IQD", "IRR", "ISK", "JEP", "JMD", "JOD", "JPY",
        "KES", "KGS", "KHR", "KMF", "KPW", "KRW", "KWD", "KYD", "KZT", "LAK", "LBP", "LKR", "LRD", "LSL", "LTC", "LTL",
        "LVL", "LYD", "MAD", "MDL", "MGA", "MKD", "MMK", "MNT", "MOP", "MRO", "MUR", "MVR", "MWK", "MXN", "MYR", "MZN",
        "NAD", "NGN", "NIO", "NOK", "NPR", "NZD", "OMR", "OP", "PAB", "PEN", "PGK", "PHP", "PKR", "PLN", "PYG", "QAR",
        "RON", "RSD", "RUB", "RWF", "SAR", "SBD", "SCR", "SDG", "SEK", "SGD", "SHP", "SLL", "SOL", "SOS", "SRD", "STD",
        "SVC", "SYP", "SZL", "THB", "TJS", "TMT", "TND", "TOP", "TRY", "TTD", "TWD", "TZS", "UAH", "UGX", "USD", "UYU",
        "UZS", "VEF", "VND", "VUV", "WST", "XAF", "XAG", "XAU", "XCD", "XDR", "XOF", "XPD", "XPF", "XPT", "XRP", "YER",
        "ZAR", "ZMK", "ZMW", "ZWL"
    ];

    public string CurrencyPair => "USD";
    public long LastTimeStamp { get; set; }

    public double GetValue(string key)
    {
        if (!cache.TryGetValue(key, out double cachedRate))
            throw new ArgumentException($"The key {key} was not found in the cache.");

        return cachedRate;
    }

    public void SetValue(string key, double rate)
    {
        cache.Set(key, rate, TimeSpan.FromMinutes(2));
    }

    public Dictionary<string, double> GetAllRates()
    {
        var rates = new Dictionary<string, double>();
        foreach (var currency in AvailableCurrencies)
        {
            rates.Add(currency, GetValue(currency));
        }
        return rates;
    }
}