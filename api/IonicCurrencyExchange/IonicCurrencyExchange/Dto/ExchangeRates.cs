namespace IonicCurrencyExchange.Dto;

public record ExchangeRates(
    long Timestamp,
    string CurrencyPair,
    Dictionary<string, double> Rates
);