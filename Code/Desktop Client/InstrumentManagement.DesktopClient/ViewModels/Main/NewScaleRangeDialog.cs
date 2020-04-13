namespace InstrumentManagement.DesktopClient.ViewModels.Main
{
    using InstrumentManagement.Data.Scales;
    using InstrumentManagement.Windows;
    using InstrumentManagement.Windows.DialogHandler;
    using InstrumentManagement.Windows.FocusHandler;
    using MaterialDesignThemes.Wpf;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    /// <summary>
    /// A <see cref="ViewModel"/> containing all functionalities for the <see cref="Views.Main.NewScaleRangeDialog"/>
    /// </summary>
    public class NewScaleRangeDialogViewModel : ViewModel, IDialogViewModel, IRequestFocus
    {
        /// <summary>
        /// Gets or sets a list of inputed <see cref="ScaleRange"/> for the new <see cref="Scale"/>
        /// </summary>
        public ICollection<ScaleRange> Ranges { get; set; }

        /// <summary>
        /// Gets or sets a queue of messeges for the <see cref="Snackbar"/>
        /// </summary>
        public SnackbarMessageQueue MessageQueue { get; set; }

        private ScaleRange newScaleRange;

        /// <summary>
        /// Gets or sets a <see cref="ScaleRange"/> that is currently being inputed
        /// </summary>
        public ScaleRange NewScaleRange
        {
            get
            {
                return newScaleRange;
            }
            set
            {
                newScaleRange = value;
                NotifyPropertyChanged(nameof(NewScaleRange));
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NewScaleRangeDialogViewModel"/> class
        /// </summary>
        /// <param name="scaleRanges">A list of all <see cref="ScaleRange"/> from the data store</param>
        public NewScaleRangeDialogViewModel(ICollection<ScaleRange> scaleRanges, IDialogHostViewModel dialogHostViewModel)
        {
            NewScaleRange = new ScaleRange();
            Ranges = new ObservableCollection<ScaleRange>(scaleRanges);

            MessageQueue = new SnackbarMessageQueue();

            DialogHostViewModel = dialogHostViewModel;
        }

        #region IRequestFocus Members

        public event EventHandler<FocusRequestedEventArgs> FocusRequested;

        /// <summary>
        /// Requesting a component from a View to be focused
        /// </summary>
        /// <param name="propertyName">A property to be focused</param>
        protected virtual void OnFocusRequested(string propertyName)
        {
            FocusRequested?.Invoke(this, new FocusRequestedEventArgs(propertyName));
        }

        #endregion

        #region IDialogViewModel Members

        public IDialogHostViewModel DialogHostViewModel { get; set; }

        public ICommand ConfirmCommand
        {
            get
            {
                return new ActionCommand(a => ConfirmDialog(), p => NewScaleRange.IsValid);
            }
        }

        public void ConfirmDialog()
        {
            if (NewScaleRange.UpperValue <= NewScaleRange.LowerValue)
            {
                MessageQueue.Enqueue("Gornja granica mora biti veća od donje granice");
                return;
            }

            ScaleRange scaleRange = Ranges.SingleOrDefault(p => p.UpperValue == NewScaleRange.UpperValue);

            if (scaleRange != null)
            {
                MessageQueue.Enqueue("Opseg sa unetom gornjom granicom već postoji");
                OnFocusRequested(nameof(NewScaleRange.UpperValue));
            }
            else
            {
                DialogHostViewModel.MessageQueue.Enqueue("Uspešno ste uneli novi opseg");
                DialogResult = true;
            }
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
