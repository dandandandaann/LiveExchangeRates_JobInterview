namespace IonicCurrencyExchange.Dto;

public class FxRatesDto
{
    public required bool Success { get; set; }
    public string? Terms { get; set; }
    public string? Privacy { get; set; }
    public required long Timestamp { get; set; }
    public string? Date { get; set; }
    public required string Base { get; set; }
    public required Dictionary<string, double> Rates { get; set; }
}