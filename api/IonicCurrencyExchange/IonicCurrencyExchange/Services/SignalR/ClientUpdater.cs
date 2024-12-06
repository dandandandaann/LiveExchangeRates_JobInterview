using IonicCurrencyExchange.Dto;
using IonicCurrencyExchange.Mappers;
using Microsoft.AspNetCore.SignalR;

namespace IonicCurrencyExchange.Services.SignalR;

public class ClientUpdater(IExchangeRateMapper mapper, IHubContext<ExchangeRatesHub> hubContext): IClientUpdater
{
    public async Task SendExchangeRates()
    {
        ExchangeRatesDto result = mapper.FromCache();
        // Send the data to connected clients
        await hubContext.Clients.All.SendAsync("transferExchangeRateData", result);

    }

}