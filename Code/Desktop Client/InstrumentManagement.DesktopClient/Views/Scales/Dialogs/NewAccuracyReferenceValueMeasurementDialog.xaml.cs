namespace InstrumentManagement.DesktopClient.Views.Scales.Dialogs
{
    using InstrumentManagement.DesktopClient.ViewModels.Scales.Dialogs;
    using System.Linq;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for NewAccuracyReferenceValueMeasurementDialog.xaml
    /// </summary>
    public partial class NewAccuracyReferenceValueMeasurementDialog : UserControl
    {
        public NewAccuracyReferenceValueMeasurementDialog()
        {
            InitializeComponent();
        }

        private void ConfirmButton_MouseEnter(object sender, MouseEventArgs e)
        {
            var errors = (from c in
                  (from object i in ResultsDataGrid.ItemsSource
                   select ResultsDataGrid.ItemContainerGenerator.ContainerFromItem(i))
                          where c != null
                          select Validation.GetHasError(c))
             .FirstOrDefault(x => x);

            (DataContext as NewAccuracyReferenceValueMeasurementDialogViewModel).HasDataGridErrors = errors;
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckPoint.Text = null;
        }
    }
}
