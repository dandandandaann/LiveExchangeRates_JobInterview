using FluentAssertions;
using IonicCurrencyExchange;
using IonicCurrencyExchange.Services.Cache;
using Microsoft.Extensions.Caching.Memory;

namespace UnitTest;

public class ExchangeRatesCacheTests
{
    private readonly ExchangeRatesCache _exchangeRatesCache = new(new MemoryCache(new MemoryCacheOptions()));

    [Fact]
    public void GetValue_ReturnsCorrectRate_WhenKeyExists()
    {
        // Arrange
        var key = "EUR";
        var expectedRate = 1.2d;

        // Act
        _exchangeRatesCache.SetValue(key, expectedRate);
        var rate = _exchangeRatesCache.GetValue(key);

        // Assert
        rate.Should().Be(expectedRate);
    }

    [Fact]
    public void GetValue_ThrowsException_WhenKeyDoesNotExist()
    {
        // Arrange
        var key = "GBP";

        // Act & Assert
        Action act = () => _exchangeRatesCache.GetValue(key);
        act.Should().Throw<ArgumentException>()
           .WithMessage($"The key {key} was not found in the cache.");
    }

    [Fact]
    public void SetValue_AddsValueToCacheAndAvailableCurrencies()
    {
        // Arrange
        var key = "JPY";
        var rate = 110d;

        // Act
        _exchangeRatesCache.SetValue(key, rate);
        var result = _exchangeRatesCache.GetValue(key);

        // Assert
        result.Should().Be(rate);
        _exchangeRatesCache.AvailableCurrencies.Should().Contain(key);
    }

    [Fact]
    public void GetAllRates_ReturnsAllCachedRates()
    {
        // Arrange
        var currency1 = "USD";
        var currency2 = "EUR";
        var rate1 = 1.0d;
        var rate2 = 0.9d;

        _exchangeRatesCache.SetValue(currency1, rate1);
        _exchangeRatesCache.SetValue(currency2, rate2);

        // Act
        var rates = _exchangeRatesCache.GetAllRates();

        // Assert
        rates.Should().HaveCount(2);
        rates[currency1].Should().Be(rate1);
        rates[currency2].Should().Be(rate2);
    }

    [Fact]
    public void Property_CurrencyPair_ReturnsCorrectValue()
    {
        // Act
        var currencyPair = _exchangeRatesCache.CurrencyPair;

        // Assert
        currencyPair.Should().Be("USD");
    }

    [Fact]
    public void Property_LastTimeStamp_CanBeGetAndSet()
    {
        // Arrange
        var timestamp = DateTime.Now.Ticks;

        // Act
        _exchangeRatesCache.LastTimestamp = timestamp;

        // Assert
        _exchangeRatesCache.LastTimestamp.Should().Be(timestamp);
    }
}