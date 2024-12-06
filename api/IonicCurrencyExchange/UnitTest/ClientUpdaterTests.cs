using IonicCurrencyExchange.Dto;
using IonicCurrencyExchange.Mappers;
using IonicCurrencyExchange.Services.ClientUpdater;
using IonicCurrencyExchange.Services.RabbitMq;
using IonicCurrencyExchange.Services.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTest;

public class ClientUpdaterTests
{
    private readonly Mock<IExchangeRateMapper> _mockMapper = new();
    private readonly Mock<IHubContext<ExchangeRatesHub>> _mockHubContext = new();
    private readonly Mock<IClientProxy> _mockClientProxy = new();
    private readonly Mock<IRabbitMqSetup> _mockRabbitMqSetup = new();
    private readonly Mock<ILogger<ClientUpdater>> _mockLogger = new();
    private readonly ClientUpdater _clientUpdater;

    public ClientUpdaterTests()
    {
        _mockHubContext.Setup(c => c.Clients.All).Returns(_mockClientProxy.Object);
        _clientUpdater = new ClientUpdater(_mockMapper.Object, _mockHubContext.Object, _mockRabbitMqSetup.Object, _mockLogger.Object);
    }

    [Fact]
    public void SendExchangeRates_ShouldSendExchangeRatesToClients()
    {
        // Arrange
        var fooExchangeRatesDto = new ExchangeRatesDto(1234567890, "ABC",
            new Dictionary<string, double> { { "USD", 1.0 }, { "EUR", 0.9 } });
        _mockMapper.Setup(m => m.FromCache()).Returns(fooExchangeRatesDto);

        // Act
        _clientUpdater.SendExchangeRates("dummy message");

        // Assert
        _mockClientProxy.Verify(
            c => c.SendCoreAsync(
                "transferExchangeRateData",
                It.Is<object[]>(args =>
                    args.Length == 1 &&
                    (ExchangeRatesDto)args[0] == fooExchangeRatesDto
                ),
                It.IsAny<CancellationToken>()
            ),
            Times.Once);
    }

    [Fact]
    public async Task StartAsync_ShouldSubscribeToQueue()
    {
        // Act
        await _clientUpdater.StartAsync(default);

        // Assert
        _mockRabbitMqSetup.Verify(r => r.Subscribe("exchangeRatesQueue", It.IsAny<Action<string>>()), Times.Once());
    }

    [Fact]
    public async Task StopAsync_ShouldCompleteSuccessfully()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        // Act
        await _clientUpdater.StopAsync(cancellationToken);

        // Assert
        Assert.True(true); // Nothing to see here.
    }
}