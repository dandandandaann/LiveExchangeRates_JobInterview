namespace IonicCurrencyExchange.Model;

public record ExchangeRates(
    long Timestamp,
    string CurrencyPair,
    Dictionary<string, double> Rates
);