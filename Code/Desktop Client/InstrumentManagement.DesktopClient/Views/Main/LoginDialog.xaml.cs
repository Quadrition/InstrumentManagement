namespace InstrumentManagement.DesktopClient.Views.Main
{
    using InstrumentManagement.DesktopClient.ViewModels.Main;
    using InstrumentManagement.Windows.FocusHandler;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for LoginDialog.xaml
    /// </summary>
    public partial class LoginDialog : UserControl
    {
        public LoginDialog()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UsernameComboBox.Focus();

            IRequestFocus focus = (IRequestFocus)DataContext;

            focus.FocusRequested += OnFocusRequested;
        }

        private void OnFocusRequested(object sender, FocusRequestedEventArgs e)
        {
            var viewModel = (LoginDialogViewModel)DataContext;

            switch (e.PropertyName)
            {
                case nameof(viewModel.Password):
                    PasswordBox.Focus();
                    PasswordBox.SelectAll();
                    break;
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                ((LoginDialogViewModel)DataContext).Password = ((PasswordBox)sender).Password;
            }
        }
    }
}
