namespace InstrumentManagement.DesktopClient
{
    using InstrumentManagement.DesktopClient.ViewModels.Main;
    using InstrumentManagement.DesktopClient.Views.Main;
    using System.Windows;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //TODO izmeni title od svakog window
            base.OnStartup(e);

            var window = new MainWindow();
            MainWindow = window;

            var viewModel = new MainWindowViewModel();
            window.DataContext = viewModel;

            window.ShowDialog();
        }
    }
}
