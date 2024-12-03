namespace IonicCurrencyExchange.Dto;

public class FxRatesDto
{
    public bool success { get; set; }
    public string terms { get; set; }
    public string privacy { get; set; }
    public long timestamp { get; set; }
    public string date { get; set; }
    public string @base { get; set; }
    public Dictionary<string, double> rates { get; set; }
}