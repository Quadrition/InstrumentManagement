namespace InstrumentManagement.DesktopClient.ViewModels.Main
{
    using System.Windows.Input;
    using InstrumentManagement.Windows;
    using InstrumentManagement.Windows.DialogHandler;

    /// <summary>
    /// A <see cref="ViewModel"/> containing all functionalities for the <see cref="Views.Main.EditInfoDialog>"/>
    /// </summary>
    public class EditInfoDialogViewModel : ViewModel, IDialogViewModel
    {
        private string info;

        /// <summary>
        /// Gets or sets an info message which will be displayed
        /// </summary>
        public string Info
        {
            get
            {
                return info;
            }
            set
            {
                info = value;
                NotifyPropertyChanged(nameof(Info));
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="EditInfoDialogViewModel"/> class
        /// </summary>
        /// <param name="pastInfo">Previous info that has been displayed</param>
        /// <param name="dialogHostViewModel">A <see cref="IDialogHostViewModel"/> from which the <see cref="EditInfoDialogViewModel"/> is opened</param>
        public EditInfoDialogViewModel(string pastInfo, IDialogHostViewModel dialogHostViewModel)
        {
            Info = pastInfo;

            DialogHostViewModel = dialogHostViewModel;
        }

        #region IDialogViewModel Members

        public ICommand ConfirmCommand
        {
            get
            {
                return new ActionCommand(a => DialogResult = true);
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return new ActionCommand(a => DialogResult = false);
            }
        }

        public IDialogHostViewModel DialogHostViewModel { get; set; }

        private bool? dialogResult;

        public bool? DialogResult
        {
            get
            {
                return dialogResult;
            }
            set
            {
                dialogResult = value;
                NotifyPropertyChanged(nameof(DialogResult));

                DialogHostViewModel.IsDialogOpened = false;
            }
        }

        #endregion
    }
}
