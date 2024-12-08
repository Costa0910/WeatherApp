using MobileApp.Services;
using MobileApp.Models;
using MobileApp.ViewModels;
using System.Collections.ObjectModel;

namespace MobileApp.Pages;

public partial class FavoritesPage : ContentPage
{
    private readonly ApiService _service;
    private readonly FavoriteService _favoriteService;
    public ObservableCollection<FavoriteModel> Favorites { get; set; }

    public FavoritesPage(ApiService service, FavoriteService favoriteService)
    {
        _service = service;
        _favoriteService = favoriteService;
        Favorites = new ObservableCollection<FavoriteModel>();
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadFavoritesAsync();
    }

    private async Task LoadFavoritesAsync()
    {
        try
        {
            Loader.IsVisible = true;
            FavoritesCollection.IsVisible = false;

            var favorites = await _favoriteService.ReadAllAsync();

            Favorites.Clear();
            foreach (var favorite in favorites)
            {
                Favorites.Add(favorite);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error",
                $"Failed to load favorites: {ex.Message}", "OK");
        }
        finally
        {
            Loader.IsVisible = false;
            FavoritesCollection.IsVisible = true;
        }
    }

    private async void SeeMoreClicked(object sender, EventArgs e)
    {
        if (sender is ImageButton button &&
            button.CommandParameter is FavoriteModel favorite)
        {
            var searchModel = CreateSearchModel(favorite);
            var cityModel = new CityWeatherViewModel(_service);
            await Navigation.PushAsync(
                new CityWeatherPage(cityModel, searchModel));
        }
    }

    private SearchResultModel CreateSearchModel(FavoriteModel favorite)
    {
        return new SearchResultModel
        {
            Id = favorite.Id,
            Name = favorite.Name,
            Country = favorite.Country,
            Lat = favorite.Lat,
            Lon = favorite.Lon
        };
    }

    private async void OnRemoveClicked(object sender, EventArgs e)
    {
        if (sender is ImageButton button &&
            button.CommandParameter is FavoriteModel favorite)
        {
            bool confirm = await DisplayAlert("Remove",
                $"Are you sure you want to remove '{favorite.Name}, {favorite.Country}'?",
                "Yes",
                "No");
            if (confirm)
            {
                try
                {
                    await _favoriteService.DeleteAsync(favorite.Id);
                    Favorites.Remove(favorite);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error",
                        $"Failed to remove item: {ex.Message}", "OK");
                }
            }
        }
    }
}
