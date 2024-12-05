namespace IonicCurrencyExchange.Services.Cache;

public interface IExchangeRatesCache
{
    HashSet<string> AvailableCurrencies { get; }
    string CurrencyPair { get; }
    long LastTimestamp { get; set; }
    double GetValue(string key);
    void SetValue(string key, double rate);
    Dictionary<string, double> GetAllRates();
}