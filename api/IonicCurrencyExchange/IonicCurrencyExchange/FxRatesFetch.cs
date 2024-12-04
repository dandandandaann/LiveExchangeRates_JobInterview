using IonicCurrencyExchange.Dto;

namespace IonicCurrencyExchange;

public class FxRatesFetch(ILogger<FxRatesFetch> logger, IHttpClientFactory httpClientFactory, ExchangeRatesCache cache) : IHostedService
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
            if (await response.Content.ReadFromJsonAsync<FxRatesDto>() is not { } content)
            {
                var data = await response.Content.ReadAsStringAsync();
                logger.LogError("Failed to parse fetched data from API. Data: {data}", data);
                return;
            }

            logger.LogInformation("Successfully fetched data from API.");

            foreach (var rate in content.rates)
            {
                cache.SetValue(rate.Key, rate.Value);
            }

            cache.LastTimeStamp = content.timestamp;
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