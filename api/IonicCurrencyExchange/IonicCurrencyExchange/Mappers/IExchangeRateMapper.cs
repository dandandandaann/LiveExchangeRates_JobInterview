using IonicCurrencyExchange.Dto;
using IonicCurrencyExchange.Services.Cache;

namespace IonicCurrencyExchange.Mappers;

/// <summary>
/// Represents a mapper for exchange rates that retrieves data from a cache.
/// </summary>
public interface IExchangeRateMapper
{
    /// <summary>
    /// Retrieves exchange rates from the cache.
    /// </summary>
    /// <returns>An instance of <see cref="ExchangeRatesDto"/> containing the cached exchange rates.</returns>
    public ExchangeRatesDto FromCache();
}