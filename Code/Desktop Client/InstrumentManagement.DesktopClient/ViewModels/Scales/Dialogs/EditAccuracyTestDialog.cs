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
    /// A <see cref="ViewModel"/> containing all functionalities for the <see cref="Views.Scales.Dialogs.EditAccuracyTestDialog"/>
    /// </summary>
    public class EditAccuracyTestDialogViewModel : ViewModel, IDialogViewModel
    {
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
        /// Initializes a new instance of <see cref="EditAccuracyTestDialogViewModel"/> class
        /// </summary>
        /// <param name="account">An <see cref="Account"/> which did the <see cref="Test"/></param>
        /// <param name="referenceValue"> A <see cref="ScaleAccuracyTest"/> which needs to be edited</param>
        /// <param name="dialogHostViewModel">A <see cref="IDialogHostViewModel"/> from which the <see cref="EditAccuracyTestDialogViewModel"/> is opened</param>
        public EditAccuracyTestDialogViewModel(ScaleAccuracyTest test, Account account, IDialogHostViewModel dialogHostViewModel)
        {
            Test = test;

            Measurements = new ObservableCollection<ScaleAccuracyTestMeasurement>(test.Measurements);

            Date = test.Date;

            DialogHostViewModel = dialogHostViewModel;

            MessageQueue = new SnackbarMessageQueue();
        }

        private DateTime date;

        /// <summary>
        /// Gets or sets a <see cref="DateTime"/> for the test
        /// </summary>
        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
                NotifyPropertyChanged(nameof(Date));
            }
        }

        private ICollection<ScaleAccuracyTestMeasurement> measurements;

        /// <summary>
        /// Gets or sets a list of <see cref="ScaleAccuracyTestMeasurement"/> of the <see cref="Test"/>
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

            for (int i = 0; i < Measurements.Count; i++)
            {
                Test.Measurements.ElementAt(i).Result = Measurements.ElementAt(i).Result;
            }
            Test.Date = Date;

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
