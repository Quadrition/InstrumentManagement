namespace InstrumentManagement.DesktopClient.Views.Administration
{
    using InstrumentManagement.DesktopClient.ViewModels.Administration;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for NewAccountDialog.xaml
    /// </summary>
    public partial class NewAccountDialog : UserControl
    {
        public NewAccountDialog()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((NewAccountDialogViewModel)this.DataContext).NewAccount.Password = ((PasswordBox)sender).Password;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            PasswordBox.Password = null;
        }
    }
}
