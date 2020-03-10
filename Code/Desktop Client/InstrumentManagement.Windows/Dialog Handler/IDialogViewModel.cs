namespace InstrumentManagement.Windows.DialogHandler
{
    /// <summary>
    /// Represents an interface with basic dialog functionalities
    /// </summary>
    public interface IDialogViewModel
    {
        /// <summary>
        /// A <see cref="IDialogHostViewModel"/> from which the <see cref="IDialogViewModel"/> is opened
        /// </summary>
        IDialogHostViewModel DialogHostViewModel { get; set; }

        /// <summary>
        /// Gets or sets a dialog result and closes the dialog
        /// </summary>
        bool? DialogResult { get; set; }
    }
}
