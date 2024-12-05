namespace IonicCurrencyExchange.Dto;

/// <summary>
/// Represents the exchange rates data transfer object.
/// </summary>
/// <param name="Timestamp">The timestamp of the exchange rates.</param>
/// <param name="CurrencyPair">The currency pair for which the exchange rates are provided.</param>
/// <param name="Rates">A dictionary containing the exchange rates with the currency code as the key and the rate as the value.</param>
public record ExchangeRatesDto(
    long Timestamp,
    string CurrencyPair,
    Dictionary<string, double> Rates
);