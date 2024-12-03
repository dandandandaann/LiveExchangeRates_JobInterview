using System.Globalization;
using IonicCurrencyExchange.Dto;
using IonicCurrencyExchange.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy => policy.WithOrigins("http://localhost:4200") // Angular app
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowLocalhost");
}

app.UseHttpsRedirection();

app.MapGet("/exchangeratedata", async () =>
    {
        // TODO: move api request (to a controller?)
        using var client = new HttpClient();
        const string url = "https://api.fxratesapi.com/latest";
        HttpResponseMessage response = await client.GetAsync(url);

        if (!response.IsSuccessStatusCode ||
            await response.Content.ReadFromJsonAsync<FxRatesDto>() is not FxRatesDto content) // TODO: better error handling?
        {
            return Results.Problem(
                detail: $"Status code: {response.StatusCode}",
                statusCode: 500,
                title: "Internal Server Error"
            );
        }

        var result = new ExchangeRates(
            content.timestamp,
            content.@base,
            content.rates
        );
        return Results.Ok(result);
    })
    .WithName("GetExchangeRateData");

app.Run();