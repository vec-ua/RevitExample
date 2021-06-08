using RevitApp.ViewModels;
using RevitApp.Views;
using System.Windows;

namespace RevitApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs args)
        {
            base.OnStartup(args);

            MainViewModel mainViewModel = new MainViewModel();
            MainView mainView = new MainView() { DataContext = mainViewModel };
            mainView.Show();
        }
    }
}
