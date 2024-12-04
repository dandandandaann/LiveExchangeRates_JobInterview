using Microsoft.Extensions.Caching.Memory;

namespace IonicCurrencyExchange;

public class ExchangeRatesCache(IMemoryCache cache)
{
    public readonly HashSet<string> AvailableCurrencies = new();

    public string CurrencyPair => "USD";
    public long LastTimestamp { get; set; }

    public double GetValue(string key)
    {
        if (!cache.TryGetValue(key, out double cachedRate))
            throw new ArgumentException($"The key {key} was not found in the cache.");

        return cachedRate;
    }

    public void SetValue(string key, double rate)
    {
        cache.Set(key, rate, TimeSpan.FromMinutes(2));
        AvailableCurrencies.Add(key);
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