namespace InstrumentManagement.DesktopClient.ViewModels.Scales.Dialogs
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using InstrumentManagement.Data;
    using InstrumentManagement.Data.Scales.Accuracy;
    using InstrumentManagement.Windows;
    using InstrumentManagement.Windows.DialogHandler;
    using MaterialDesignThemes.Wpf;
    using Data.Scales.Calibration;

    /// <summary>
    /// A <see cref="ViewModel"/> containing all functionalities for the <see cref="Views.Dialogs.NewAccuracyReferenceValueDialog"/>
    /// </summary>
    public class NewAccuracyReferenceValueDialogViewModel : ViewModel, IDialogViewModel, IDialogHostViewModel
    {
        private ScaleAccuracyReferenceValue referenceValue;

        /// <summary>
        /// Gets or sets a <see cref="ScaleAccuracyReferenceValue"/> that is currently being inputed
        /// </summary>
        public ScaleAccuracyReferenceValue ReferenceValue
        {
            get
            {
                return referenceValue;
            }
            set
            {
                referenceValue = value;
                NotifyPropertyChanged(nameof(ReferenceValue));
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NewAccuracyReferenceValueDialogViewModel"/> class
        /// </summary>
        /// <param name="calibrationAccuracy">A <see cref="ScaleCalibrationAccuracy"/> to which the <see cref="ReferenceValue"/> belongs</param>
        /// <param name="dialogHostViewModel">A <see cref="IDialogHostViewModel"/> from which the <see cref="NewAccuracyReferenceValueDialogViewModel"/> is opened</param>
        public NewAccuracyReferenceValueDialogViewModel(ScaleCalibrationAccuracy calibrationAccuracy, IDialogHostViewModel dialogHostViewModel)
        {
            Measurements = new ObservableCollection<ScaleAccuracyReferenceValueMeasurement>();

            ReferenceValue = new ScaleAccuracyReferenceValue()
            {
                Accuracy = calibrationAccuracy
            };

            DialogHostViewModel = dialogHostViewModel;

            MessageQueue = new SnackbarMessageQueue();
        }

        #region Measurements Members

        private ICollection<ScaleAccuracyReferenceValueMeasurement> measurements;

        /// <summary>
        /// Gets or sets a list of <see cref="ScaleAccuracyReferenceValueMeasurement"/> of the <see cref="ReferenceValue"/>
        /// </summary>
        public ICollection<ScaleAccuracyReferenceValueMeasurement> Measurements
        {
            get
            {
                return measurements;
            }
            set
            {
                measurements = value;
                NotifyPropertyChanged(nameof(Measurements));
            }
        }

        private ScaleAccuracyReferenceValueMeasurement selectedMeasurement;

        /// <summary>
        /// Gets or sets a selected <see cref="ScaleAccuracyReferenceValueMeasurement"/> from the <see cref="Measurements"/>
        /// </summary>
        public ScaleAccuracyReferenceValueMeasurement SelectedMeasurement
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
        /// Gets an <see cref="ICommand"/> for opening a <see cref="Views.Scales.Dialogs.NewAccuracyReferenceValueMeasurementDialog"/>
        /// </summary>
        public ICommand OpenNewMeasurementDialogCommand
        {
            get
            {
                return new ActionCommand(a => OpenNewMeasurementDialog());
            }
        }

        /// <summary>
        /// Opens a <see cref="Views.Scales.Dialogs.NewAccuracyReferenceValueMeasurementDialog"/>
        /// </summary>
        private void OpenNewMeasurementDialog()
        {
            DialogViewModel = new NewAccuracyReferenceValueMeasurementDialogViewModel(ReferenceValue, this);

            IsDialogOpened = true;
        }

        /// <summary>
        /// Gets an <see cref="ICommand"/> for removing a <see cref="SelectedMeasurement"/> from the <see cref="Measurements"/>
        /// </summary>
        public ICommand RemoveMeasurementCommand
        {
            get
            {
                return new ActionCommand(a => RemoveMeasurement(), p => SelectedMeasurement != null);
            }
        }

        /// <summary>
        /// Removes a <see cref="SelectedMeasurement"/> from the <see cref="Measurements"/>
        /// </summary>
        private void RemoveMeasurement()
        {
            ReferenceValue.Measurements.Remove(SelectedMeasurement);
            Measurements.Remove(SelectedMeasurement);

            MessageQueue.Enqueue("Uspešno ste obrisali merenje");
        }

        #endregion

        #region IDialogHostViewModel Members

        private bool isDialogOpened;

        public bool IsDialogOpened
        {
            get
            {
                return isDialogOpened;
            }
            set
            {
                isDialogOpened = value;
                NotifyPropertyChanged(nameof(IsDialogOpened));

                if (!isDialogOpened && DialogViewModel != null)
                {
                    if (DialogViewModel.DialogResult.HasValue && DialogViewModel.DialogResult == true)
                    {
                        switch (DialogViewModel)
                        {
                            case NewAccuracyReferenceValueMeasurementDialogViewModel newMeasurementViewModel:
                                ReferenceValue.Measurements.Add(newMeasurementViewModel.Measurement);
                                Measurements.Add(newMeasurementViewModel.Measurement);
                                break;
                        }
                    }

                    DialogViewModel = null;
                }
            }
        }

        public SnackbarMessageQueue MessageQueue { get; set; }

        private IDialogViewModel dialogViewModel;

        public IDialogViewModel DialogViewModel
        {
            get
            {
                return dialogViewModel;
            }
            set
            {
                dialogViewModel = value;
                NotifyPropertyChanged(nameof(DialogViewModel));
            }
        }

        #endregion

        #region IDialogViewModel Members

        public ICommand ConfirmCommand
        {
            get
            {
                return new ActionCommand(a => ConfirmDialog(), p => Measurements.Count != 0);
            }
        }

        public void ConfirmDialog()
        {
            ReferenceValue.Measurements = new HashSet<ScaleAccuracyReferenceValueMeasurement>(Measurements);

            DialogHostViewModel.MessageQueue.Enqueue("Uspešno ste uneli referentnu vrednost");
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
