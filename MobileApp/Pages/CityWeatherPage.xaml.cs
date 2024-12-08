using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileApp.Models;
using MobileApp.Services;
using MobileApp.ViewModels;

namespace MobileApp.Pages;

public partial class CityWeatherPage : ContentPage
{
    private readonly CityWeatherViewModel _viewModel;

    public CityWeatherPage(CityWeatherViewModel
        viewModel, SearchResultModel selectedCity)
    {
        viewModel.Longitude = selectedCity.Lon;
        viewModel.Latitude = selectedCity.Lat;
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }
}
