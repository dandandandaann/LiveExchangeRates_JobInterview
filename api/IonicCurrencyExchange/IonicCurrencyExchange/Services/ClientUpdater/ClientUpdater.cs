using IonicCurrencyExchange.Mappers;
using IonicCurrencyExchange.Services.RabbitMq;
using IonicCurrencyExchange.Services.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace IonicCurrencyExchange.Services.ClientUpdater;

/// <summary>
/// The ClientUpdater class is responsible for updating clients with the latest exchange rates.
/// It implements the <see cref="IClientUpdater"/> and <see cref="IHostedService"/> interfaces.
/// </summary>
/// <param name="mapper">An instance of <see cref="IExchangeRateMapper"/> used to map exchange rate data from cache.</param>
/// <param name="hubContext">An instance of <see cref="IHubContext{T}"/> used to send data to connected clients.</param>
/// <param name="rabbitMqSetup">An instance of <see cref="IRabbitMqSetup"/> used to subscribe to RabbitMQ queues.</param>
/// <param name="logger">An instance of <see cref="ILogger{T}"/> used for logging information.</param>
public class ClientUpdater(
    IExchangeRateMapper mapper,
    IHubContext<ExchangeRatesHub> hubContext,
    IRabbitMqSetup rabbitMqSetup,
    ILogger<ClientUpdater> logger) : IClientUpdater, IHostedService

{
    /// <summary>
    /// Sends the exchange rates to all connected clients.
    /// </summary>
    /// <param name="message">The message containing exchange rate information.</param>
    public void SendExchangeRates(string message)
    {
        var result = mapper.FromCache();
        // Send the data to connected clients
        hubContext.Clients.All.SendAsync("transferExchangeRateData", result).Wait();
        logger.LogInformation("Exchange rates sent to clients.");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Subscribe to RabbitMQ queue
        rabbitMqSetup.Subscribe("exchangeRatesQueue", SendExchangeRates);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}