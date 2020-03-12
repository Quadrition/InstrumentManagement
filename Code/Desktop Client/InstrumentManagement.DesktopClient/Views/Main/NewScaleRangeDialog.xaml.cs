namespace InstrumentManagement.DesktopClient.Views.Main
{
    using InstrumentManagement.DesktopClient.ViewModels.Main;
    using InstrumentManagement.Windows.FocusHandler;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for NewScaleRangeDialog.xaml
    /// </summary>
    public partial class NewScaleRangeDialog : UserControl
    {
        public NewScaleRangeDialog()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            UpperValueTextBox.Text = null;
            LowerValueTextBox.Text = null;
            GraduateTextBox.Text = null;

            IRequestFocus focus = (IRequestFocus)DataContext;
            focus.FocusRequested += OnFocusRequested;
        }

        private void OnFocusRequested(object sender, FocusRequestedEventArgs e)
        {
            var viewModel = (NewScaleRangeDialogViewModel)DataContext;

            switch (e.PropertyName)
            {
                case nameof(viewModel.NewScaleRange.UpperValue):
                    UpperValueTextBox.Focus();
                    UpperValueTextBox.SelectAll();
                    break;
            }
        }
    }
}
