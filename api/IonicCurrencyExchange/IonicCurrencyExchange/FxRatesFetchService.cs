using IonicCurrencyExchange.Dto;
using Microsoft.AspNetCore.SignalR;

namespace IonicCurrencyExchange;

public class FxRatesFetchService(
    ILogger<FxRatesFetchService> logger,
    IHttpClientFactory httpClientFactory,
    ExchangeRatesCache cache,
    IHubContext<ExchangeRatesHub> hubContext) : IHostedService
{
    private Timer? _timer;
    private readonly TimeSpan _repeatInterval = TimeSpan.FromSeconds(60);

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

            await SendHubData();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while fetching new exchange rates.");
        }
    }

    async Task SendHubData()
    {
        var rates = cache.GetAllRates();
        var result = new ExchangeRates(
            cache.LastTimestamp,
            cache.CurrencyPair,
            rates
        );
        // Send the data to connected clients
        await hubContext.Clients.All.SendAsync("transferExchangeRateData", result);
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