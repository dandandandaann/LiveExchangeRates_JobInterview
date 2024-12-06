using Microsoft.Extensions.Caching.Memory;

namespace IonicCurrencyExchange.Services.Cache;

/// <summary>
/// Represents a cache for storing and retrieving exchange rates.
/// </summary>
public class ExchangeRatesCache(IMemoryCache cache) : IExchangeRatesCache
{
    /// <summary>
    /// Gets the set of available currencies in the cache.
    /// </summary>
    public HashSet<string> AvailableCurrencies { get; } = new();

    /// <summary>
    /// Gets the default currency pair.
    /// </summary>
    public string CurrencyPair { get; } = "USD";

    /// <summary>
    /// Gets or sets the timestamp of the last cache update.
    /// </summary>
    public long LastTimestamp { get; set; }

    /// <summary>
    /// Retrieves the exchange rate for the specified currency key.
    /// </summary>
    /// <param name="key">The currency key to retrieve the exchange rate for.</param>
    /// <returns>The exchange rate for the specified currency key.</returns>
    /// <exception cref="ArgumentException">Thrown when the specified key is not found in the cache.</exception>
    public double GetValue(string key)
    {
        if (!cache.TryGetValue(key, out double cachedRate))
            throw new ArgumentException($"The key {key} was not found in the cache.");

        return cachedRate;
    }

    /// <summary>
    /// Sets the exchange rate for the specified currency key.
    /// </summary>
    /// <param name="key">The currency key to set the exchange rate for.</param>
    /// <param name="rate">The exchange rate to set.</param>
    public void SetValue(string key, double rate)
    {
        cache.Set(key, rate);
        AvailableCurrencies.Add(key);
    }

    /// <summary>
    /// Retrieves all exchange rates from the cache.
    /// </summary>
    /// <returns>A dictionary containing all currency keys and their corresponding exchange rates.</returns>
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
