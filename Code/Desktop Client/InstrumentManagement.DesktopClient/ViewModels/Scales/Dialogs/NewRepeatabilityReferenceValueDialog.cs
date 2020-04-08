namespace InstrumentManagement.DesktopClient.ViewModels.Scales.Dialogs
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.Windows.Input;
    using InstrumentManagement.Windows;
    using MaterialDesignThemes.Wpf;
    using System.Linq;
    using InstrumentManagement.Data.Scales.Repeatability;
    using InstrumentManagement.Data.Scales.Calibration;
    using InstrumentManagement.Windows.DialogHandler;

    /// <summary>
    /// A <see cref="ViewModel"/> containing all functionalities for the <see cref="Views.Scales.Dialogs.NewRepeatabilityReferenceValueDialog"/>
    /// </summary>
    public class NewRepeatabilityReferenceValueDialogViewModel : ViewModel, IDialogViewModel
    {
        /// <summary>
        /// Gets or sets a message queue for the <see cref="Snackbar"/>
        /// </summary>
        public SnackbarMessageQueue MessageQueue { get; set; }

        private ScaleRepeatabilityReferenceValue referenceValue;

        /// <summary>
        /// Gets or sets a <see cref="ReferenceValue"/> which needs to be added
        /// </summary>
        public ScaleRepeatabilityReferenceValue ReferenceValue
        {
            get
            {
                return referenceValue;
            }
            set
            {
                referenceValue = value;
                NotifyPropertyChanged(nameof(ScaleRepeatabilityReferenceValue));
            }
        }

        private bool hasDataGridErrors;

        /// <summary>
        /// Gets or sets an indicator if the <see cref="System.Windows.Controls.DataGrid"/>
        /// </summary>
        public bool HasDataGridErrors
        {
            get
            {
                return hasDataGridErrors;
            }
            set
            {
                hasDataGridErrors = value;
                NotifyPropertyChanged(nameof(HasDataGridErrors));
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NewRepeatabilityReferenceValueDialogViewModel"/> class
        /// </summary>
        /// <param name="calibrationRepeatability">A <see cref="ScaleCalibrationRepeatability"/> to which the reference value belongs</param>
        /// <param name="baseViewModel">A <see cref="IDialogHostViewModel"/> from which the <see cref="NewRepeatabilityReferenceValueDialogViewModel"/> is opened</param>
        public NewRepeatabilityReferenceValueDialogViewModel(ScaleCalibrationRepeatability calibrationRepeatability, IDialogHostViewModel baseViewModel)
        {
            DialogHostViewModel = baseViewModel;

            ReferenceValue = new ScaleRepeatabilityReferenceValue()
            {
                Repeatability = calibrationRepeatability
            };

            Measurements = new ObservableCollection<ScaleRepeatabilityReferenceValueMeasurement>();

            ChangeMeasurements(6);

            MessageQueue = new SnackbarMessageQueue();

            HasDataGridErrors = false;
        }

        #region Manual Reference Value Members

        private bool isManualReferenceValue;

        /// <summary>
        /// Gets or sets an indicator if <see cref="ReferenceValue.ReferenceValue"/> is typed manually
        /// </summary>
        public bool IsManualReferenceValue
        {
            get
            {
                return isManualReferenceValue;
            }
            set
            {
                isManualReferenceValue = value;
                NotifyPropertyChanged(nameof(IsManualReferenceValue));
            }
        }

        private double manualReferenceValue;

        /// <summary>
        /// Gets or sets a <see cref="ReferenceValue.ReferenceValue"/> which is entered manually
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Neispravna referentna vrednost")]
        [Required(ErrorMessage = "Referentna vrednost je obavezna")]
        public double ManualReferenceValue
        {
            get
            {
                return manualReferenceValue;
            }
            set
            {
                manualReferenceValue = value;
                NotifyPropertyChanged(nameof(ManualReferenceValue));
            }
        }

        #endregion

        #region Measurements Members

        private ICollection<ScaleRepeatabilityReferenceValueMeasurement> measurements;

        /// <summary>
        /// Gets or sets a list of <see cref="ScaleRepeatabilityReferenceValueMeasurement"/>
        /// </summary>
        public ICollection<ScaleRepeatabilityReferenceValueMeasurement> Measurements
        {
            get
            {
                return measurements;
            }
            set
            {
                measurements = value;
                NotifyPropertyChanged(nameof(ScaleRepeatabilityReferenceValueMeasurement));
            }
        }

        /// <summary>
        /// Gets a command for changing to 6 <see cref="ScaleRepeatabilityReferenceValueMeasurement"/>
        /// </summary>
        public ICommand ChangeTo6MeasurementsCommand
        {
            get
            {
                return new ActionCommand(p => ChangeMeasurements(6));
            }
        }

        /// <summary>
        /// Gets a command for changing to 10 <see cref="ScaleRepeatabilityReferenceValueMeasurement"/>
        /// </summary>
        public ICommand ChangeTo10MeasurementsCommand
        {
            get
            {
                return new ActionCommand(p => ChangeMeasurements(10));
            }
        }

        /// <summary>
        /// Changes number of measurements
        /// </summary>
        /// <param name="number">Number of <see cref="ScaleRepeatabilityReferenceValueMeasurement"/> that needs to be changed</param>
        private void ChangeMeasurements(int number)
        {
            ReferenceValue.NumberOfMeasurements = number;

            Measurements.Clear();

            for (int i = 0; i < number; i++)
            {
                Measurements.Add(new ScaleRepeatabilityReferenceValueMeasurement());
            }
        }

        #endregion

        #region IDialogViewModel Members

        public IDialogHostViewModel DialogHostViewModel { get; set; }

        public ICommand ConfirmCommand
        {
            get
            {
                return new ActionCommand(a => ConfirmDialog(), p => ReferenceValue.IsValid);
            }
        }

        public void ConfirmDialog()
        {
            bool existsNull = Measurements.FirstOrDefault(p => p.Result == null) != null;
            bool existsNonNull = Measurements.FirstOrDefault(p => p.Result != null) != null;

            if (HasDataGridErrors)
            {
                MessageQueue.Enqueue("Postoje greške u rezultatima merenja");
                return;
            }

            if (existsNull == existsNonNull)
            {
                MessageQueue.Enqueue("Niste uneli sve rezultate merenja");
                return;
            }

            if (!IsManualReferenceValue && existsNull)
            {
                MessageQueue.Enqueue("Morate uneti sve rezultate merenja");
                return;
            }

            if (!IsManualReferenceValue && OnValidate(nameof(ManualReferenceValue)) != null)
            {
                MessageQueue.Enqueue("Uneta referentna vrednost je neispravna");
                return;
            }

            if (!existsNull)
            {
                ReferenceValue.Measurements = new HashSet<ScaleRepeatabilityReferenceValueMeasurement>(Measurements);
            }

            if (IsManualReferenceValue)
            {
                ReferenceValue.ReferenceValue = ManualReferenceValue;
            }
            else
            {
                ReferenceValue.CalculateReferenceValue();
            }

            DialogHostViewModel.MessageQueue.Enqueue("Uspešno ste uneli referentnu vrednost ponovljivosti");
            DialogResult = true;
        }

        public ICommand CancelCommand
        {
            get
            {
                return new ActionCommand(p => CancelDialog());
            }
        }

        public void CancelDialog()
        {
            DialogResult = false;
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
