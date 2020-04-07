using System.Windows;
using System.Windows.Controls;

namespace InstrumentManagement.DesktopClient.Views.Scales.Dialogs
{
    /// <summary>
    /// Interaction logic for NewCalibrationAccuracyMeasurementDialog.xaml
    /// </summary>
    public partial class NewCalibrationAccuracyMeasurementDialog : UserControl
    {
        public NewCalibrationAccuracyMeasurementDialog()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            checkPointTextBox.Text = null;
            deviationTextBox.Text = null;
        }
    }
}
