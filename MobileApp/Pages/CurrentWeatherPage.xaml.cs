using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileApp.ViewModels;

namespace MobileApp.Pages;

public partial class CurrentWeatherPage : ContentPage
{
    private readonly CurrentWeatherViewModel _viewModel;

    public CurrentWeatherPage(CurrentWeatherViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }
}
