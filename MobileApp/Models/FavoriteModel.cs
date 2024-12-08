using SQLite;

namespace MobileApp.Models;

public class FavoriteModel
{
    [PrimaryKey]
    public Guid Id { get; init; }
    public string Name { get; set; }
    public string Country { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
}
