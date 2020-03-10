namespace InstrumentManagement.DesktopClient.Views.Administration
{
    using InstrumentManagement.DesktopClient.ViewModels.Administration;
    using InstrumentManagement.Windows.ClearHandler;
    using InstrumentManagement.Windows.FocusHandler;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for AdministrationDialog.xaml
    /// </summary>
    public partial class AdministrationDialog : UserControl
    {
        public AdministrationDialog()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            IRequestFocus focus = (IRequestFocus)DataContext;
            focus.FocusRequested += OnFocusRequested;

            IRequestClear clear = (IRequestClear)DataContext;
            clear.ClearRequested += OnClearRequested;
        }

        private void OnFocusRequested(object sender, FocusRequestedEventArgs e)
        {
            var viewModel = (AdministrationDialogViewModel)DataContext;

            switch (e.PropertyName)
            {
                case nameof(viewModel.OldPassword):
                    OldPasswordBox.Focus();
                    OldPasswordBox.SelectAll();
                    break;

                case nameof(viewModel.NewPassword):
                    NewPasswordBox.Focus();
                    NewPasswordBox.SelectAll();
                    break;

                case nameof(viewModel.ConfirmedNewPassword):
                    ConfirmNewPasswordBox.Focus();
                    ConfirmNewPasswordBox.SelectAll();
                    break;
            }
        }

        private void OnClearRequested(object sender, ClearRequestedEventArgs e)
        {
            var viewModel = (AdministrationDialogViewModel)DataContext;

            switch (e.PropertyName)
            {
                case nameof(viewModel.OldPassword):
                    OldPasswordBox.Clear();
                    break;

                case nameof(viewModel.NewPassword):
                    NewPasswordBox.Clear();
                    break;

                case nameof(viewModel.ConfirmedNewPassword):
                    ConfirmNewPasswordBox.Clear();
                    break;
            }
        }

        private void OldPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                ((AdministrationDialogViewModel)DataContext).OldPassword = ((PasswordBox)sender).Password;
            }
        }

        private void NewPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                ((AdministrationDialogViewModel)DataContext).NewPassword = ((PasswordBox)sender).Password;
            }
        }

        private void ConfirmNewPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                ((AdministrationDialogViewModel)DataContext).ConfirmedNewPassword = ((PasswordBox)sender).Password;
            }
        }
    }
}
