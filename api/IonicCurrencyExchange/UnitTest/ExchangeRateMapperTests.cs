using IonicCurrencyExchange.Mappers;
using IonicCurrencyExchange.Services.Cache;
using Moq;
using FluentAssertions;

namespace UnitTest
{
    public class ExchangeRateMapperTests
    {
        [Fact]
        public void FromCache_ShouldReturnExchangeRatesDto()
        {
            // Arrange
            var mockCache = new Mock<IExchangeRatesCache>();

            var expectedRates = new Dictionary<string, double> { { "USD", 1.0 }, { "EUR", 0.9 } };
            var expectedTimestamp = 1234567890;
            var expectedCurrencyPair = "ABC";

            mockCache.Setup(cache => cache.GetAllRates()).Returns(expectedRates);
            mockCache.Setup(cache => cache.LastTimestamp).Returns(expectedTimestamp);
            mockCache.Setup(cache => cache.CurrencyPair).Returns(expectedCurrencyPair);

            var mapper = new ExchangeRateMapper(mockCache.Object);

            // Act
            var result = mapper.FromCache();

            // Assert
            result.Should().NotBeNull();
            result.Timestamp.Should().Be(expectedTimestamp);
            result.CurrencyPair.Should().Be(expectedCurrencyPair);
            result.Rates.Should().BeEquivalentTo(expectedRates);
        }
    }
}