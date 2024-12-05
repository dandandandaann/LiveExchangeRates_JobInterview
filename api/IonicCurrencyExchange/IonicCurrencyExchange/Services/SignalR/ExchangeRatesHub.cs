using Microsoft.AspNetCore.SignalR;

namespace IonicCurrencyExchange.Services.SignalR;

/// <summary>
/// Represents a SignalR hub for managing exchange rate data communication.
/// </summary>
/// <param name="mapper">An instance of <see cref="ExchangeRateMapper"/> used to map exchange rate data from cache.</param>
public class ExchangeRatesHub(ExchangeRateMapper mapper) : Hub
{
    /// <summary>
    /// Called when a new connection is established with the hub.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public override async Task OnConnectedAsync()
    {
        // Send data to the connecting client
        await Clients.Caller.SendAsync("transferExchangeRateData", mapper.FromCache());

        // Call the base method so that the connection is established correctly
        await base.OnConnectedAsync();
    }
}