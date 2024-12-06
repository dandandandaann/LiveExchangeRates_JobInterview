using IonicCurrencyExchange;
using IonicCurrencyExchange.Mappers;
using IonicCurrencyExchange.Services.Cache;
using IonicCurrencyExchange.Services.FxRatesWorker;
using IonicCurrencyExchange.Services.RabbitMq;
using IonicCurrencyExchange.Services.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy => policy.WithOrigins("http://localhost:4200") // Angular app
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IExchangeRatesCache, ExchangeRatesCache>();
builder.Services.AddSingleton<IExchangeRateMapper, ExchangeRateMapper>();
builder.Services.AddSingleton<IClientUpdater, ClientUpdater>();

builder.Services.AddHostedService<FxRatesFetchService>();

builder.Services.AddSignalR();
builder.Services.AddSingleton<RabbitMqService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowLocalhost");
}

app.MapHub<ExchangeRatesHub>("/exchangerateshub");

app.Run();