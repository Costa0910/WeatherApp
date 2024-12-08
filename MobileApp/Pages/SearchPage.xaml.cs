using System.Collections.ObjectModel;
using MobileApp.Models;
using MobileApp.Services;
using MobileApp.ViewModels;

namespace MobileApp.Pages;

public partial class SearchPage : ContentPage
{
    private readonly ApiService _service;
    private readonly FavoriteService _favoriteService;
    private ObservableCollection<SearchResultModel> SearchResults { get; set; }

    public SearchPage(ApiService service, FavoriteService favoriteService)
    {
        SearchResults = [];
        _service = service;
        _favoriteService = favoriteService;
        InitializeComponent();
        ResultsCollectionView.ItemsSource = SearchResults;
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(e.NewTextValue) &&
            e.NewTextValue.Length >= 3)
        {
            SearchEntry.ReturnType
                = ReturnType.Search; // Enable keyboard search icon
        }
        else
        {
            ResultsCollectionView.IsVisible = false;
            SearchEntry.ReturnType = ReturnType.Default;
        }

        NoResultsLabel.IsVisible = false;
    }

    private async void OnSearchCompleted(object sender, EventArgs e)
    {
        string searchQuery = SearchEntry.Text;

        if (string.IsNullOrWhiteSpace(searchQuery) || searchQuery.Length < 3)
            return;

        await PerformSearch(searchQuery);
    }

    private async Task PerformSearch(string query)
    {
        // Show loading indicator and clear previous results
        LoadingIndicator.IsVisible = true;
        LoadingIndicator.IsRunning = true;
        ResultsCollectionView.IsVisible = false;
        SearchResults.Clear();

        try
        {
            var url = $"geo/1.0/direct?q={query}&limit=5";
            var results
                = await _service.GetWeatherAsync<List<SearchResultModel>>(url);


            if (!results.HasError && results.Data != null && results.Data.Any())
            {
                foreach (var result in results.Data)
                {
                    SearchResults.Add(result);
                }

                ResultsCollectionView.IsVisible = true;
                NoResultsLabel.IsVisible = false; // Hide if results are present
            }
            else
            {
                ResultsCollectionView.IsVisible = false;
                NoResultsLabel.IsVisible = true; // Show "No results found"
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Something went wrong: " + ex.Message,
                "OK");
        }
        finally
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
        }
    }

    private async void OnItemSelected(object sender,
        SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is SearchResultModel
            selectedItem)
        {
            ResultsCollectionView.SelectedItem = null;
            var viewModel = new CityWeatherViewModel(_service);
            await Navigation.PushAsync(new CityWeatherPage(viewModel,
                selectedItem));
        }
    }

    private async void OnFavoriteClicked(object sender, EventArgs e)
    {
        if (sender is ImageButton button &&
            button.CommandParameter is SearchResultModel item)
        {
            try
            {
                if (item.IsFavorite)
                {
                    await _favoriteService.DeleteAsync(item.Id);
                    item.IsFavorite = false;
                }
                else
                {
                    var favoritePlace = CreateFavoritePlace(item);
                    await _favoriteService.CreateAsync(favoritePlace);
                    item.IsFavorite = true;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error",
                    "Something went wrong: " + ex.Message,
                    "OK");
            }
        }
    }

    private FavoriteModel CreateFavoritePlace(SearchResultModel item)
    {
        return new FavoriteModel
        {
            Id = item.Id,
            Name = item.Name,
            Country = item.Country,
            Lat = item.Lat,
            Lon = item.Lon
        };
    }
}
