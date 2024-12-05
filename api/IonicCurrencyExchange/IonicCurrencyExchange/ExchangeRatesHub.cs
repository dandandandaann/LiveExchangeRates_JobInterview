using IonicCurrencyExchange.Dto;
using Microsoft.AspNetCore.SignalR;

namespace IonicCurrencyExchange;

public class ExchangeRatesHub : Hub
{
    public async Task SendExchangeRateData(ExchangeRatesCache cache)
    {
        var rates = cache.GetAllRates();
        var result = new ExchangeRates(
            cache.LastTimestamp,
            cache.CurrencyPair,
            rates
        );

        // Send data to all connected clients
        await Clients.All.SendAsync("transferExchangeRateData", result);
    }
}