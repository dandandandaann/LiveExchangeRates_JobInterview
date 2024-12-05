// FILE: ExchangeRatesHubTests.cs

using IonicCurrencyExchange.Dto;
using IonicCurrencyExchange.Mappers;
using IonicCurrencyExchange.Services.SignalR;
using Microsoft.AspNetCore.SignalR;
using Moq;

namespace UnitTest
{
    public class ExchangeRatesHubTests
    {
        [Fact]
        public async Task OnConnectedAsync_ShouldSendExchangeRateDataToCaller()
        {
            // Arrange
            var mockClients = new Mock<IHubCallerClients>();
            var mockCaller = new Mock<ISingleClientProxy>();
            var mockMapper = new Mock<IExchangeRateMapper>();

            mockClients.Setup(clients => clients.Caller).Returns(mockCaller.Object);

            var fooExchangeRatesDto = new ExchangeRatesDto(1234567890, "ABC",
                new Dictionary<string, double> { { "USD", 1.0 }, { "EUR", 0.9 } });
            mockMapper.Setup(mapper => mapper.FromCache()).Returns(fooExchangeRatesDto);

            var hub = new ExchangeRatesHub(mockMapper.Object)
            {
                Clients = mockClients.Object
            };

            // Act
            await hub.OnConnectedAsync();

            // Assert
            mockCaller.Verify(
                caller => caller.SendCoreAsync("transferExchangeRateData",
                    It.Is<object[]>(o => o.Length == 1 && o[0] as ExchangeRatesDto == fooExchangeRatesDto),
                    default),
                Times.Once);

            mockMapper.Verify(mapper => mapper.FromCache(), Times.Once);
        }
    }
}