﻿namespace InstrumentManagement.DesktopClient.Views.Scales.Dialogs
{
    using InstrumentManagement.DesktopClient.ViewModels.Scales.Dialogs;
    using System.Linq;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for NewRepeatabilityTestDialog.xaml
    /// </summary>
    public partial class NewRepeatabilityTestDialog : UserControl
    {
        public NewRepeatabilityTestDialog()
        {
            InitializeComponent();
        }

        private void ConfirmButton_MouseEnter(object sender, MouseEventArgs e)
        {
            var errors = (from c in
                  (from object i in MeasurementsDataGrid.ItemsSource
                   select MeasurementsDataGrid.ItemContainerGenerator.ContainerFromItem(i))
                          where c != null
                          select Validation.GetHasError(c))
             .FirstOrDefault(x => x);

            (DataContext as NewRepeatabilityTestDialogViewModel).HasDataGridErrors = errors;
        }
    }
}
