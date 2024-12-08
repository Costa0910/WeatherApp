using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace MobileApp.Models;

public class SearchResultModel : INotifyPropertyChanged
{
    [JsonIgnore] public Guid Id { get; init; }
    private bool _isFavorite;
    public string Name { get; set; }
    public string? State { get; set; }
    public string Country { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }

    public bool IsFavorite
    {
        get => _isFavorite;
        set
        {
            if (_isFavorite == value)
                return;

            _isFavorite = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public SearchResultModel()
    {
        Id = Guid.NewGuid();
    }

    protected virtual void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this,
            new PropertyChangedEventArgs(propertyName));
    }
}
