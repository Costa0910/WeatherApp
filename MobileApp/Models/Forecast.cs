namespace MobileApp.Models;

public class Forecast
{
    public long Dt { get; set; }
    public Main Main { get; set; }
    public List<DayWeatherModel> Weather { get; set; }
    public Clouds Clouds { get; set; }
    public Wind Wind { get; set; }
    public int Visibility { get; set; }
    public double Pop { get; set; }
    public Sys Sys { get; set; }
    public string Dt_txt { get; set; }
}