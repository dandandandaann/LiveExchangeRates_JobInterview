using IonicCurrencyExchange;
using IonicCurrencyExchange.Dto;
using IonicCurrencyExchange.Model;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ExchangeRatesCache>();

builder.Services.AddHostedService<FxRatesFetch>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowLocalhost");
}

app.MapGet("/exchangeratedata", (ExchangeRatesCache cache) =>
    {
        var rates = cache.GetAllRates();
        var result = new ExchangeRates(
            cache.LastTimeStamp,
            cache.CurrencyPair,
            rates
        );

        return Results.Ok(result);
    })
    .WithName("GetExchangeRateData");

app.Run();