namespace InstrumentManagement.DesktopClient.Views.Scales.Main.Accuracy
{
    using InstrumentManagement.DesktopClient.ViewModels.Scales.Main;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for Tests.xaml
    /// </summary>
    public partial class Tests : UserControl
    {
        public Tests()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as ScaleWindowViewModel).PrintAccuracyDataGrid(AccuracyDataGrid, false);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            (DataContext as ScaleWindowViewModel).PrintAccuracyDataGrid(SinglePointAccuracyDataGrid, true);
        }
    }
}
