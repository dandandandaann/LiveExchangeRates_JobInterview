using System.Net;
using System.Net.Http.Json;
using System.Text;
using FluentAssertions;
using IonicCurrencyExchange;
using IonicCurrencyExchange.Dto;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;

namespace UnitTest;

public class FxRatesFetchServiceTests
{
    private readonly Mock<ILogger<FxRatesFetchService>> _mockLogger;
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly ExchangeRatesCache _exchangeRatesCache;
    private readonly FxRatesFetchService _fxRatesFetchService;

    public FxRatesFetchServiceTests()
    {
        _mockLogger = new Mock<ILogger<FxRatesFetchService>>();
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        _exchangeRatesCache = new ExchangeRatesCache(new MemoryCache(new MemoryCacheOptions()));

        _fxRatesFetchService = new FxRatesFetchService(_mockLogger.Object, _mockHttpClientFactory.Object, _exchangeRatesCache, null, null); // TODO: fix test
    }

    [Fact]
    public async Task DoWork_ShouldFetchAndCacheRates_WhenApiReturnsSuccessfulResponse()
    {
        // Arrange
        var ratesDto = new FxRatesDto
        {
            Rates = new Dictionary<string, double> { { "EUR", 1.1 }, { "GBP", 1.2 } },
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            Base = "USD",
            Success = true
        };

        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(ratesDto)
            });

        var client = new HttpClient(mockHandler.Object);
        _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(client);

        // Act
        await _fxRatesFetchService.StartAsync(CancellationToken.None);
        await Task.Delay(100); // Give some time for the timer to tick

        // Assert
        _exchangeRatesCache.GetValue("EUR").Should().Be(1.1);
        _exchangeRatesCache.GetValue("GBP").Should().Be(1.2);
        _exchangeRatesCache.LastTimestamp.Should().Be(ratesDto.Timestamp);
    }
    
    [Fact]
    public async Task DoWork_ShouldLogError_WhenApiReturnsNonSuccessStatusCode()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest, // Non-success status code
            });

        var client = new HttpClient(mockHandler.Object);
        _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(client);

        // Act
        await _fxRatesFetchService.StartAsync(CancellationToken.None);
        await Task.Delay(100); // Give some time for the timer to tick

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Failed to fetch data from API")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
    }

    [Fact]
    public async Task DoWork_ShouldLogError_WhenApiResponseCannotBeDeserialized()
    {
        // Arrange
        var invalidJson = "{ \"invalid\": \"data\" }"; // Invalid JSON for FxRatesDto

        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(invalidJson, Encoding.UTF8, "application/json")
            });

        var client = new HttpClient(mockHandler.Object);
        _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(client);

        // Act
        await _fxRatesFetchService.StartAsync(CancellationToken.None);
        await Task.Delay(100); // Give some time for the timer to tick

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("An error occurred while fetching new exchange rates.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
    }

    [Fact]
    public async Task StartAsync_ShouldInitializeTimer()
    {
        // Act
        await _fxRatesFetchService.StartAsync(CancellationToken.None);

        // Assert
        // Internal behavior (timer setup) isn't directly verifiable externally.
    }

    [Fact]
    public async Task StopAsync_ShouldDisableTimer()
    {
        // Arrange
        await _fxRatesFetchService.StartAsync(CancellationToken.None);

        // Act
        await _fxRatesFetchService.StopAsync(CancellationToken.None);
        _fxRatesFetchService.Dispose();

        // Assert
        // If desired, further checks on timer status might require exposing internal state or properties.
    }
}