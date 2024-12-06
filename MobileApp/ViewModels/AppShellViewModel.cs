// using System.ComponentModel;
// using MobileApp.Pages;
// using MobileApp.Services;
//
// namespace MobileApp.ViewModels;
//
// public class AppShellViewModel : INotifyPropertyChanged
// {
//     public event PropertyChangedEventHandler PropertyChanged;
//
//     public Command LogoutCommand { get; }
//
//     public AppShellViewModel(ApiService service)
//     {
//         // Command for logout
//         LogoutCommand = new Command(async () =>
//         {
//             bool confirm = await Application.Current?.MainPage?.DisplayAlert(
//                 "Logout", "Are you sure you want to logout?", "Yes", "No");
//             if (confirm)
//             {
//                 // Perform logout
//                 Application.Current.MainPage = new LoginPage(service);
//             }
//         });
//     }
// }
