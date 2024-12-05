using IonicCurrencyExchange.Dto;
using IonicCurrencyExchange.Services.Cache;

namespace IonicCurrencyExchange;

public class ExchangeRateMapper(IExchangeRatesCache cache)
{
    public ExchangeRates FromCache()
    {
        var rates = cache.GetAllRates();
        var result = new ExchangeRates(
            cache.LastTimestamp,
            cache.CurrencyPair,
            rates
        );
        return result;
    }
}