using IonicCurrencyExchange.Dto;
using IonicCurrencyExchange.Services.Cache;
using IonicCurrencyExchange.Services.SignalR;

namespace IonicCurrencyExchange.Services.FxRatesWorker;

/// <summary>
/// Service to fetch exchange rates periodically from fxratesapi and update the cache and connected clients.
/// </summary>
/// <param name="logger">The logger instance for logging information and errors.</param>
/// <param name="httpClientFactory">The factory to create HTTP clients for making API requests.</param>
/// <param name="cache">The cache to store the fetched exchange rates.</param>
/// <param name="clientUpdater">The client updater responsible for sending the updated exchange rates to connected clients.</param>
public class FxRatesFetchService(
    ILogger<FxRatesFetchService> logger,
    IHttpClientFactory httpClientFactory,
    IExchangeRatesCache cache,
    IClientUpdater clientUpdater) : IHostedService
{
    private Timer? _timer;
    private readonly TimeSpan _repeatInterval = TimeSpan.FromSeconds(30);

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, _repeatInterval);

        return Task.CompletedTask;
    }

    private async void DoWork(object? state)
    {
        try
        {
            using var client = httpClientFactory.CreateClient();
            const string apiUrl = "https://api.fxratesapi.com/latest";

            // Make the API request
            var response = await client.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("Failed to fetch data from API. Status code: {StatusCode}", response.StatusCode);
                return;
            }

            var content = await response.Content.ReadFromJsonAsync<FxRatesDto>();

            logger.LogInformation("Successfully fetched data from API.");

            foreach (var rate in content!.Rates)
            {
                cache.SetValue(rate.Key, rate.Value);
            }

            cache.LastTimestamp = content.Timestamp;

            await clientUpdater.SendExchangeRates();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while fetching new exchange rates.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}