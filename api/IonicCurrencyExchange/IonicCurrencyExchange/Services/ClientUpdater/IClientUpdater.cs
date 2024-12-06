namespace IonicCurrencyExchange.Services.ClientUpdater;

/// <summary>
/// Defines the contract for updating clients with exchange rate information.
/// </summary>
public interface IClientUpdater
{
    /// <summary>
    /// Sends exchange rate information to the client.
    /// </summary>
    /// <param name="message">The message containing exchange rate information.</param>
    void SendExchangeRates(string message);
}