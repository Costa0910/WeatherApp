using MobileApp.Pages;
using MobileApp.Services;
using MobileApp.ViewModels;

namespace MobileApp;

public partial class AppShell : Shell
{
    public AppShell(ApiService service)
    {
        InitializeComponent();
        // Set the BindingContext to the ViewModel
        BindingContext = new AppShellViewModel(service);


        // Register routes for additional pages
        Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
    }
}
