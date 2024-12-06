namespace MobileApp.Models;

/// <summary>
///    Weather forecast model.
/// </summary>
public class WeatherForecastModel
{
    public string Cod { get; set; }
    public int Message { get; set; }
    public int Cnt { get; set; }
    public List<Forecast> List { get; set; }
    public City City { get; set; }
}
