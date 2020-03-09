namespace InstrumentManagement.Windows
{
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Represents an interface with basic functionalities for a dialog host
    /// </summary>
    public interface IDialogHostViewModel
    {
        /// <summary>
        /// Gets or sets an indicator if a dialog is opened
        /// </summary>
        bool IsDialogOpened { get; set; }

        /// <summary>
        /// Gets or sets a message queue for a <see cref="Snackbar"/>
        /// </summary>
        SnackbarMessageQueue MessageQueue { get; set; }

        /// <summary>
        /// Gets or sets a viewModel for the dialog
        /// </summary>
        IDialogViewModel DialogViewModel { get; set; }
    }
}
