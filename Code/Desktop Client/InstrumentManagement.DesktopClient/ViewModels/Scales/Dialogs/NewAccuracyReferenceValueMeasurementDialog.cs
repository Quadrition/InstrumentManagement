namespace InstrumentManagement.DesktopClient.ViewModels.Scales.Dialogs
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using InstrumentManagement.Data.Scales.Accuracy;
    using InstrumentManagement.Windows;
    using InstrumentManagement.Windows.DialogHandler;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// A <see cref="ViewModel"/> containing all functionalities for the <see cref="Views.NewScaleAccuracyReferenceValueDialog"/>
    /// </summary>
    public class NewAccuracyReferenceValueMeasurementDialogViewModel : ViewModel, IDialogViewModel
    {
        /// <summary>
        /// Gets or sets a message queue for the <see cref="Snackbar"/>
        /// </summary>
        public SnackbarMessageQueue MessageQueue { get; set; }

        private ScaleAccuracyReferenceValueMeasurement measurement;

        /// <summary>
        /// Gets or sets a <see cref="Measurement"/> that is currently being inputed
        /// </summary>
        public ScaleAccuracyReferenceValueMeasurement Measurement
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

        private ICollection<ScaleAccuracyReferenceValueMeasurementResult> results;

        /// <summary>
        /// Gets or sets a list of <see cref="ScaleAccuracyReferenceValueMeasurementResult"/> for the <see cref="Measurement"/>
        /// </summary>
        public ICollection<ScaleAccuracyReferenceValueMeasurementResult> Results
        {
            get
            {
                return results;
            }
            set
            {
                results = value;
                NotifyPropertyChanged(nameof(Results));
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NewAccuracyReferenceValueMeasurementDialogViewModel"/> class
        /// </summary>
        /// <param name="referenceValue">A <see cref="ScaleAccuracyReferenceValue"/> to which the <see cref="Measurement"/> belongs</param>
        /// <param name="dialogHostViewModel">A <see cref="IDialogHostViewModel"/> from which the <see cref="NewAccuracyReferenceValueMeasurementDialogViewModel"/> is opened</param>
        public NewAccuracyReferenceValueMeasurementDialogViewModel(ScaleAccuracyReferenceValue referenceValue, IDialogHostViewModel dialogHostViewModel)
        {
            Results = new ObservableCollection<ScaleAccuracyReferenceValueMeasurementResult>();

            for (int i = 0; i < 10; i++)
            {
                Results.Add(new ScaleAccuracyReferenceValueMeasurementResult());
            }

            Measurement = new ScaleAccuracyReferenceValueMeasurement()
            {
                AccuracyReferenceValue = referenceValue
            };

            DialogHostViewModel = dialogHostViewModel;

            MessageQueue = new SnackbarMessageQueue();
        }

        private bool hasDataGridErrors;

        /// <summary>
        /// Gets or sets an indicator if the <see cref="System.Windows.Controls.DataGrid"/> has errors
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

        #region IDialogViewModel Members

        public ICommand ConfirmCommand
        {
            get
            {
                return new ActionCommand(a => ConfirmDialog(), p => CanConfirmCommand);
            }
        }

        private bool CanConfirmCommand
        {
            get
            {
                foreach (ScaleAccuracyReferenceValueMeasurementResult result in Results)
                {
                    if (!result.IsValid)
                        return false;
                }

                return Measurement.IsValid;
            }
        }

        public void ConfirmDialog()
        {
            if (HasDataGridErrors)
            {
                MessageQueue.Enqueue("Postoje greške u rezultatima merenja");
                return;
            }

            Measurement.Results = new HashSet<ScaleAccuracyReferenceValueMeasurementResult>(Results);
            Measurement.CalculateReferenceValue();

            DialogHostViewModel.MessageQueue.Enqueue("Uspešno ste dodali novo merenje");
            DialogResult = true;
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
