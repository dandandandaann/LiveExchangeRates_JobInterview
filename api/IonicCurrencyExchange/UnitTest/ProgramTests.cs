// TODO: figure out how to test minimal api endpoints
// using System.Net.Http.Json;
// using Microsoft.AspNetCore.Mvc.Testing;
// using FluentAssertions;
// using IonicCurrencyExchange.Dto;
//
// namespace UnitTest;
//
// internal class ExchangeRateApiTests : IClassFixture<WebApplicationFactory<Program>>
// {
//     private readonly HttpClient _client;
//
//     public ExchangeRateApiTests(WebApplicationFactory<Program> factory)
//     {
//         _client = factory.CreateClient();
//     }
//
//     [Fact]
//     public async Task GetExchangeRateData_ShouldReturnOkResponseWithCorrectData()
//     {
//         // Act
//         var response = await _client.GetAsync("/exchangeratedata");
//
//         // Assert
//         response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
//
//         var exchangeRates = await response.Content.ReadFromJsonAsync<ExchangeRatesDto>();
//
//         exchangeRates.Should().NotBeNull();
//         exchangeRates!.Rates.Should().NotBeEmpty();
//         // exchangeRates!.Timestamp.Should().BeCloseTo(System.DateTimeOffset.Now, TimeSpan.FromMinutes(1));
//         // Add additional assertions based on expected results structure
//     }
// }