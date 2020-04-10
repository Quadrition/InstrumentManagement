namespace InstrumentManagement.DesktopClient.ViewModels.Scales.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using InstrumentManagement.Data.Accounts;
    using InstrumentManagement.Data.Scales.Accuracy;
    using InstrumentManagement.Windows;
    using InstrumentManagement.Windows.DialogHandler;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// A <see cref="ViewModel"/> containing all functionalities for the <see cref="Views.NewScaleAccuracyTestDialog"/>
    /// </summary>
    public class NewAccuracyTestDialogViewModel : ViewModel, IDialogViewModel
    {
        private bool potentialErrors;

        private ScaleAccuracyTest test;

        /// <summary>
        /// Gets or sets a <see cref="ScaleAccuracyTest"/> that is currently being inputed
        /// </summary>
        public ScaleAccuracyTest Test
        {
            get
            {
                return test;
            }
            set
            {
                test = value;
                NotifyPropertyChanged(nameof(Test));
            }
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

        /// <summary>
        /// Gets or sets a message queue for the <see cref="Snackbar"/>
        /// </summary>
        public SnackbarMessageQueue MessageQueue { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="NewAccuracyTestDialogViewModel"/> class
        /// </summary>
        /// <param name="account">An <see cref="Account"/> which did the <see cref="Test"/></param>
        /// <param name="referenceValue"> A <see cref="ScaleAccuracyReferenceValue"/> to which the <see cref="Test"/> belongs</param>
        /// <param name="dialogHostViewModel">A <see cref="IDialogHostViewModel"/> from which the <see cref="NewAccuracyTestDialogViewModel"/> is opened</param>
        public NewAccuracyTestDialogViewModel(ScaleAccuracyReferenceValue referenceValue, Account account, IDialogHostViewModel dialogHostViewModel)
        {
            Measurements = new ObservableCollection<ScaleAccuracyTestMeasurement>();

            foreach (ScaleAccuracyReferenceValueMeasurement accuracyMeasurement in referenceValue.Measurements)
            {
                Measurements.Add(new ScaleAccuracyTestMeasurement()
                {
                    ReferenceValueMeasurement = accuracyMeasurement,
                });
            }

            Test = new ScaleAccuracyTest()
            {
                Account = account,
                ReferenceValue = referenceValue,
                Number = (short)(referenceValue.Tests.Count + 1),
                Date = DateTime.Now
            };

            DialogHostViewModel = dialogHostViewModel;

            MessageQueue = new SnackbarMessageQueue();

            potentialErrors = true;
        }

        private ICollection<ScaleAccuracyTestMeasurement> measurements;

        /// <summary>
        /// Gets or sets a list of <see cref="RepReferenceValue.AccRefValMeasurement"/> of the <see cref="Test"/>
        /// </summary>
        public ICollection<ScaleAccuracyTestMeasurement> Measurements
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

        /// <summary>
        /// Confirms that user checked results
        /// </summary>
        void ConfirmResults()
        {
            potentialErrors = false;
            ConfirmDialog();
        }

        #region IDialogHostViewModel Members

        public ICommand ConfirmCommand
        {
            get
            {
                return new ActionCommand(a => ConfirmDialog(), p => Test.IsValid);
            }
        }

        public void ConfirmDialog()
        {
            if (HasDataGridErrors)
            {
                MessageQueue.Enqueue("Postoje greške u rezultatima merenja");
                return;
            }

            Test.Measurements = new HashSet<ScaleAccuracyTestMeasurement>(Measurements);

            if (potentialErrors)
            {
                foreach (ScaleAccuracyTestMeasurement measurement in Test.Measurements)
                {
                    if (Math.Abs(measurement.Result - measurement.ReferenceValueMeasurement.Results.Select(result => result.Result).Average()) > 2 * ScaleAccuracyReferenceValue.Coefficient * measurement.ReferenceValueMeasurement.AccuracyReferenceValue.Accuracy.Calibration.Repeatability.ReferenceValue.ReferenceValue)
                    {
                        MessageQueue.Enqueue("Da li ste sigurni u tačnost unetih rezultata?", "Da", () => ConfirmResults());
                        return;
                    }
                }
            }

            Test.CalculateResults();

            DialogResult = true;
            DialogHostViewModel.MessageQueue.Enqueue("Uspešno ste uneli novi test");
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
