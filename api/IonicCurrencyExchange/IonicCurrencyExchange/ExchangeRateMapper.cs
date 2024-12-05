using IonicCurrencyExchange.Dto;
using IonicCurrencyExchange.Services.Cache;

namespace IonicCurrencyExchange;

/// <summary>
/// Represents a mapper for exchange rates that retrieves data from a cache.
/// </summary>
/// <param name="cache">The cache that stores exchange rates.</param>
public class ExchangeRateMapper(IExchangeRatesCache cache)
{
    /// <summary>
    /// Retrieves exchange rates from the cache.
    /// </summary>
    /// <returns>An instance of <see cref="ExchangeRatesDto"/> containing the cached exchange rates.</returns>
    public ExchangeRatesDto FromCache()
    {
        var rates = cache.GetAllRates();
        var result = new ExchangeRatesDto(
            cache.LastTimestamp,
            cache.CurrencyPair,
            rates
        );
        return result;
    }
}