using System.Globalization;
using IonicCurrencyExchange.Dto;

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
        string url = "https://api.fxratesapi.com/latest";
        HttpResponseMessage response = await client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return Results.Problem(
                detail: $"Error: {response.StatusCode}",
                statusCode: 500,
                title: "Internal Server Error"
            );
        }

        var content = await response.Content.ReadFromJsonAsync<FxRatesDto>();

        return Results.Ok(content);
    })
    .WithName("GetExchangeRateData");

app.Run();
