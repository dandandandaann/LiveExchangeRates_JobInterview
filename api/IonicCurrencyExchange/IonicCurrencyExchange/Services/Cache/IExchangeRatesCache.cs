namespace IonicCurrencyExchange.Services.Cache;

/// <summary>
/// Interface for caching exchange rates.
/// </summary>
public interface IExchangeRatesCache
{
    /// <summary>
    /// Gets the set of available currencies.
    /// </summary>
    HashSet<string> AvailableCurrencies { get; }

    /// <summary>
    /// Gets the currency pair.
    /// </summary>
    string CurrencyPair { get; }

    /// <summary>
    /// Gets or sets the timestamp of the last update.
    /// </summary>
    long LastTimestamp { get; set; }

    /// <summary>
    /// Gets the exchange rate for the specified key.
    /// </summary>
    /// <param name="key">The key representing the currency pair.</param>
    /// <returns>The exchange rate for the specified key.</returns>
    double GetValue(string key);

    /// <summary>
    /// Sets the exchange rate for the specified key.
    /// </summary>
    /// <param name="key">The key representing the currency pair.</param>
    /// <param name="rate">The exchange rate to set.</param>
    void SetValue(string key, double rate);

    /// <summary>
    /// Gets all exchange rates.
    /// </summary>
    /// <returns>A dictionary containing all exchange rates.</returns>
    Dictionary<string, double> GetAllRates();
}