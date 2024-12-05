using IonicCurrencyExchange.Dto;
using Microsoft.AspNetCore.SignalR;

namespace IonicCurrencyExchange;

public class ExchangeRatesHub(ExchangeRateMapper mapper) : Hub
{
    public override async Task OnConnectedAsync()
    {
        // Fetch data that you want to send to the client
        await Clients.All.SendAsync("transferExchangeRateData", mapper.FromCache());
        var result = "";

        // Send data to the connecting client
        await Clients.Caller.SendAsync("ReceiveExchangeRateData", result);

        // Call the base method so that the connection is established correctly
        await base.OnConnectedAsync();
    }
}