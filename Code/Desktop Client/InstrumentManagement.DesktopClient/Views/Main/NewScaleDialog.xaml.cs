namespace InstrumentManagement.DesktopClient.Views.Main
{
    using InstrumentManagement.DesktopClient.ViewModels.Main;
    using InstrumentManagement.Windows.FocusHandler;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for NewScaleDialog.xaml
    /// </summary>
    public partial class NewScaleDialog : UserControl
    {
        public NewScaleDialog()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            IRequestFocus focus = (IRequestFocus)DataContext;
            focus.FocusRequested += OnFocusRequested;
        }

        private void OnFocusRequested(object sender, FocusRequestedEventArgs e)
        {
            var viewModel = (NewScaleDialogViewModel)DataContext;

            switch (e.PropertyName)
            {
                case nameof(viewModel.NewScale.SerialNumber):
                    SerialNumberTextBox.Focus();
                    SerialNumberTextBox.SelectAll();
                    break;
            }
        }
    }
}
