namespace InstrumentManagement.DesktopClient.ViewModels.Scales.Dialogs
{
    using InstrumentManagement.Data.Scales;
    using InstrumentManagement.Data.Scales.Calibration;
    using InstrumentManagement.Windows;
    using InstrumentManagement.Windows.DialogHandler;
    using MaterialDesignThemes.Wpf;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    /// <summary>
    /// A <see cref="ViewModel"/> containing all functionalities for the <see cref="Views.Scales.Dialogs.NewCalibrationDialog"/> View
    /// </summary>
    public class NewCalibrationDialogViewModel : ViewModel, IDialogViewModel, IDialogHostViewModel
    {
        /// <summary>
        /// Gets or sets a queue of messeges for the <see cref="Snackbar"/>
        /// </summary>
        public SnackbarMessageQueue MessageQueue { get; set; }

        private ScaleCalibration newCalibration;

        /// <summary>
        /// Gets or sets a <see cref="ScaleCalibration"/> that need to be inputed
        /// </summary>
        public ScaleCalibration NewCalibration
        {
            get
            {
                return newCalibration;
            }
            set
            {
                newCalibration = value;
                NotifyPropertyChanged(nameof(NewCalibration));
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NewCalibrationDialogViewModel"/> class
        /// </summary>
        /// <param name="range">A <see cref="Range"/> that is related</param>
        /// <param name="baseViewModel"><An <see cref="IDialogHostViewModel"/> from which the <see cref="NewCalibrationDialogViewModel"/> is opened</param>
        public NewCalibrationDialogViewModel(ScaleRange range, IDialogHostViewModel baseViewModel)
        {
            MessageQueue = new SnackbarMessageQueue();

            NewCalibration = new ScaleCalibration()
            {
                Repeatability = new ScaleCalibrationRepeatability(),
                Accuracy = new ScaleCalibrationAccuracy(),
                Verification = new ScaleVerification(),
                Range = range,
                Number = (short)(range.Calibrations.Count + 1)
            };

            Measurements = new ObservableCollection<ScaleCalibrationAccuracyMeasurement>();

            DialogHostViewModel = baseViewModel;
        }

        #region Edit Measurements Members

        /// <summary>
        /// Gets or sets a list of measurements that are related to the calibration accuracy
        /// </summary>
        public ICollection<ScaleCalibrationAccuracyMeasurement> Measurements { get; set; }

        private bool isDialogOpened;

        /// <summary>
        /// Gets or sets an indicator if the <see cref="Views.Scales.Dialogs.NewCalibrationAccuracyMeasurementDialog"/> is opened
        /// </summary>
        public bool IsDialogOpened
        {
            get
            {
                return isDialogOpened;
            }
            set
            {
                isDialogOpened = value;
                NotifyPropertyChanged("IsDialogOpened");

                if (!isDialogOpened && DialogViewModel != null)
                {
                    if (DialogViewModel.DialogResult.HasValue && DialogViewModel.DialogResult == true)
                    {
                        Measurements.Add((DialogViewModel as NewCalibrationAccuracyMeasurementDialogViewModel).Measurement);
                    }

                    DialogViewModel = null;
                }
            }
        }

        private IDialogViewModel dialogViewModel;

        /// <summary>
        /// Gets or sets a viewModel for the <see cref="Views.Scales.Dialogs.NewCalibrationAccuracyMeasurementDialog"/>
        /// </summary>
        public IDialogViewModel DialogViewModel
        {
            get
            {
                return dialogViewModel;
            }
            set
            {
                dialogViewModel = value;
                NotifyPropertyChanged();
            }
        }

        private ScaleCalibrationAccuracyMeasurement selectedMeasurement;

        /// <summary>
        /// Gets or sets a selected <see cref="ScaleCalibrationAccuracyMeasurement"/>
        /// </summary>
        public ScaleCalibrationAccuracyMeasurement SelectedMeasurement
        {
            get
            {
                return selectedMeasurement;
            }
            set
            {
                selectedMeasurement = value;
                NotifyPropertyChanged(nameof(SelectedMeasurement));
            }
        }

        /// <summary>
        /// Gets a command for opening a <see cref="Views.Scales.Dialogs.NewCalibrationAccuracyMeasurementDialog"/>
        /// </summary>
        public ICommand OpenNewMeasurementDialogCommand
        {
            get
            {
                return new ActionCommand(a => OpenNewMeasurementDialog());
            }
        }

        /// <summary>
        /// Opens a dialog for inputing a new measurement
        /// </summary>
        private void OpenNewMeasurementDialog()
        {
            DialogViewModel = new NewCalibrationAccuracyMeasurementDialogViewModel(this);

            IsDialogOpened = true;
        }

        /// <summary>
        /// Gets a command for deleting the selected <see cref="SelectedMeasurement"/>
        /// </summary>
        public ICommand DeleteScaleRangeCalibrationAccuracyMeasurementCommand
        {
            get
            {
                return new ActionCommand(a => DeleteScaleRangeCalibrationAccuracyMeasurement(), p => SelectedMeasurement != null);
            }
        }

        /// <summary>
        /// Removes the <see cref="SelectedMeasurement"/> from the collection
        /// </summary>
        private void DeleteScaleRangeCalibrationAccuracyMeasurement()
        {
            Measurements.Remove(SelectedMeasurement);

            SelectedMeasurement = null;
        }

        #endregion

        #region IDialogViewModel Members

        public IDialogHostViewModel DialogHostViewModel { get; set; }

        public ICommand ConfirmCommand
        {
            get
            {
                return new ActionCommand(a => ConfirmDialog(), p => IsValid);
            }
        }

        /// <summary>
        /// Checks if <see cref="NewCalibration"/> is valid
        /// </summary>
        public bool IsValid
        {
            get
            {
                foreach (ScaleCalibrationAccuracyMeasurement measurement in Measurements)
                {
                    if (!measurement.IsValid)
                        return false;
                }

                return NewCalibration.IsValid;
            }
        }

        public void ConfirmDialog()
        {
            if (Measurements.Count == 0)
            {
                MessageQueue.Enqueue("Morate uneti bar 1 merenje");
                return;
            }

            NewCalibration.Accuracy.Measurements = new HashSet<ScaleCalibrationAccuracyMeasurement>(Measurements);

            DialogHostViewModel.MessageQueue.Enqueue("Uspešno ste uneli novo etaloniranje");
            DialogResult = true;
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
