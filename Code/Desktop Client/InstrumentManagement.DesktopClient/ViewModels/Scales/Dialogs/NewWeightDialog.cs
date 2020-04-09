namespace InstrumentManagement.DesktopClient.ViewModels.Scales.Dialogs
{
    using System.Windows.Input;
    using InstrumentManagement.Data.Scales;
    using InstrumentManagement.Windows;
    using InstrumentManagement.Windows.DialogHandler;

    /// <summary>
    /// A <see cref="ViewModel"/> containing all functionalities for the <see cref="Views.Scales.Dialogs.NewWeightDialog"/>
    /// </summary>
    public class NewWeightDialogViewModel : ViewModel, IDialogViewModel
    {
        private ScaleWeight newScaleWeight;

        /// <summary>
        /// Gets or sets a new <see cref="Weight"/> for inputing
        /// </summary>
        public ScaleWeight NewScaleWeight
        {
            get
            {
                return newScaleWeight;
            }
            set
            {
                newScaleWeight = value;
                NotifyPropertyChanged(nameof(NewScaleWeight));
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NewWeightDialogViewModel"/> class
        /// </summary>
        /// <param name="dialogHostViewModel">An <see cref="IDialogHostViewModel"/> from which the <see cref="NewWeightDialogViewModel"/> is opened</param>
        public NewWeightDialogViewModel(IDialogHostViewModel dialogHostViewModel)
        {
            DialogHostViewModel = dialogHostViewModel;

            NewScaleWeight = new ScaleWeight();
        }

        #region IDialogViewModel Members

        public IDialogHostViewModel DialogHostViewModel { get; set; }

        public ICommand ConfirmCommand
        {
            get
            {
                return new ActionCommand(a => ConfirmDialog(), p => NewScaleWeight.IsValid);
            }
        }

        public void ConfirmDialog()
        {
            DialogResult = true;

            DialogHostViewModel.MessageQueue.Enqueue("Uspešno ste uneli novi teg");
        }

        public ICommand CancelCommand
        {
            get
            {
                return new ActionCommand(a => DialogResult = false);
            }
        }

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
