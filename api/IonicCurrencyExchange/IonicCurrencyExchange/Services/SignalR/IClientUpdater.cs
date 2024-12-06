namespace IonicCurrencyExchange.Services.SignalR;

public interface IClientUpdater
{
    Task SendExchangeRates();
}