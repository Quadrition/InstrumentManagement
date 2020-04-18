namespace InstrumentManagement.DesktopClient.Views.Scales.Dialogs
{
    using InstrumentManagement.DesktopClient.ViewModels.Scales.Dialogs;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for NewRepeatabilityReferenceValueDialog.xaml
    /// </summary>
    public partial class NewRepeatabilityReferenceValueDialog : UserControl
    {
        public NewRepeatabilityReferenceValueDialog()
        {
            InitializeComponent();
        }

        private void ManualReferenceValueCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ReferenceValueTextBox.Text = null;
        }

        private void ConfirmButton_MouseEnter(object sender, MouseEventArgs e)
        {
            var errors = (from c in
                  (from object i in MeasurementsDataGrid.ItemsSource
                   select MeasurementsDataGrid.ItemContainerGenerator.ContainerFromItem(i))
                          where c != null
                          select Validation.GetHasError(c))
             .FirstOrDefault(x => x);

            (DataContext as NewRepeatabilityReferenceValueDialogViewModel).HasDataGridErrors = errors;
        }
    }
}
