using InstrumentManagement.DesktopClient.ViewModels.Scales.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InstrumentManagement.DesktopClient.Views.Scales.Dialogs
{
    /// <summary>
    /// Interaction logic for EditAccuracyTestDialog.xaml
    /// </summary>
    public partial class EditAccuracyTestDialog : UserControl
    {
        public EditAccuracyTestDialog()
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

            (DataContext as EditAccuracyTestDialogViewModel).HasDataGridErrors = errors;
        }
    }
}
