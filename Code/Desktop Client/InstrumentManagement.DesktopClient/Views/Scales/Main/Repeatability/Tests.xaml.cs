using InstrumentManagement.DesktopClient.ViewModels.Scales.Main;
using System.Windows;
using System.Windows.Controls;

namespace InstrumentManagement.DesktopClient.Views.Scales.Main.Repeatability
{
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
            (DataContext as ScaleWindowViewModel).PrintRepeatabilityDataGrid(RepeatabilityDataGrid);
        }
    }
}
