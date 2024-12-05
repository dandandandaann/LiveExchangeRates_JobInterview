using IonicCurrencyExchange.Dto;

namespace IonicCurrencyExchange;

public class ExchangeRateMapper(ExchangeRatesCache cache)
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