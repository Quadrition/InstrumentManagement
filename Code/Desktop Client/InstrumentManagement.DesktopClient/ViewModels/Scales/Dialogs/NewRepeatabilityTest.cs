namespace InstrumentManagement.DesktopClient.ViewModels.Scales.Dialogs
{
    using InstrumentManagement.Data.Accounts;
    using InstrumentManagement.Data.Scales.Repeatability;
    using InstrumentManagement.Windows;
    using InstrumentManagement.Windows.DialogHandler;
    using MaterialDesignThemes.Wpf;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    /// <summary>
    /// A <see cref="ViewModel"/> containing all functionalities for the <see cref="Views.Scales.Dialogs.NewRepeatabilityTestDialog"/>
    /// </summary>
    public class NewRepeatabilityTestDialogViewModel : ViewModel, IDialogViewModel
    {
        private bool potentialErrors;

        private ScaleRepeatabilityTest test;

        /// <summary>
        /// Gets or sets a <see cref="Test"/> that is currently being added
        /// </summary>
        public ScaleRepeatabilityTest Test
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
        /// Initializes a new instance of <see cref="NewRepeatabilityTestDialogViewModel"/> class
        /// </summary>
        /// <param name="referenceValue">A <see cref="ReferenceValue"/> to which the <see cref="Test"/> is being added</param>
        /// <param name="account">An <see cref="Account"/> that does the test</param>
        /// <param name="baseViewModel">An <see cref="IDialogHostViewModel"/> from which the <see cref="NewRepeatabilityTestDialogViewModel"/> is opened</param>
        public NewRepeatabilityTestDialogViewModel(ScaleRepeatabilityReferenceValue referenceValue, Account account, IDialogHostViewModel baseViewModel)
        {
            Test = new ScaleRepeatabilityTest()
            {
                Account = account,
                Number = (short)(referenceValue.Tests.Count + 1),
                Date = DateTime.Today,
                ReferenceValue = referenceValue
            };

            Measurements = new ObservableCollection<ScaleRepeatabilityTestMeasurement>();

            for (int i = 0; i < referenceValue.NumberOfMeasurements; i++)
            {
                Measurements.Add(new ScaleRepeatabilityTestMeasurement());
            }

            DialogHostViewModel = baseViewModel;

            MessageQueue = new SnackbarMessageQueue();

            potentialErrors = true;
        }

        private ICollection<ScaleRepeatabilityTestMeasurement> measurements;

        /// <summary>
        /// Gets or sets a list of <see cref="ScaleRepeatabilityTestMeasurement"/> for the <see cref="Test"/>
        /// </summary>
        public ICollection<ScaleRepeatabilityTestMeasurement> Measurements
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

        #region IDialogViewModel Members

        public IDialogHostViewModel DialogHostViewModel { get; set; }

        public ICommand ConfirmCommand
        {
            get
            {
                return new ActionCommand(p => ConfirmDialog());
            }
        }

        public void ConfirmDialog()
        {
            if (HasDataGridErrors)
            {
                MessageQueue.Enqueue("Postoje greške u rezultatima merenja");
                return;
            }

            Test.Measurements = new HashSet<ScaleRepeatabilityTestMeasurement>(Measurements);

            if (potentialErrors && Test.StandardDeviation > 2 * Test.ReferenceValue.MaxValidValue)
            {
                MessageQueue.Enqueue("Da li ste sigurni u tačnost unetih rezultata?", "Da", () => ConfirmResults());
                return;
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
