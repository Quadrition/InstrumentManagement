namespace InstrumentManagement.DesktopClient.ViewModels.Scales.Dialogs
{
    using InstrumentManagement.Data.Scales.Calibration;
    using InstrumentManagement.Windows;
    using InstrumentManagement.Windows.DialogHandler;
    using System.Windows.Input;

    /// <summary>
    /// A <see cref="ViewModel"/> containing all functionalities for NewScaleRangeCalibrationAccuracyMeasurementControl View
    /// </summary>
    public class NewCalibrationAccuracyMeasurementDialogViewModel : ViewModel, IDialogViewModel
    {
        private ScaleCalibrationAccuracyMeasurement measurement;

        /// <summary>
        /// Gets or sets a measurement that is currently inputing
        /// </summary>
        public ScaleCalibrationAccuracyMeasurement Measurement
        {
            get
            {
                return measurement;
            }
            set
            {
                measurement = value;
                NotifyPropertyChanged(nameof(Measurement));
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NewCalibrationAccuracyMeasurementDialogViewModel"/> class
        /// </summary>
        public NewCalibrationAccuracyMeasurementDialogViewModel(IDialogHostViewModel baseViewModel)
        {
            DialogHostViewModel = baseViewModel;

            Measurement = new ScaleCalibrationAccuracyMeasurement();
        }

        #region IDialogViewModel Members

        public IDialogHostViewModel DialogHostViewModel { get; set; }

        public ICommand ConfirmCommand
        {
            get
            {
                return new ActionCommand(a => ConfirmDialog(), p => Measurement.IsValid);
            }
        }

        public void ConfirmDialog()
        {
            DialogResult = true;

            DialogHostViewModel.MessageQueue.Enqueue("Uspešno ste uneli novo merenje");
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
