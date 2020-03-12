namespace InstrumentManagement.DesktopClient.ViewModels.Main
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using InstrumentManagement.Data.Scales;
    using InstrumentManagement.Windows;
    using InstrumentManagement.Windows.DialogHandler;
    using InstrumentManagement.Windows.FocusHandler;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// A <see cref="ViewModel"/> containing all functionalities for the <see cref="Views.Main.NewScaleDialog"/>
    /// </summary>
    public class NewScaleDialogViewModel : ViewModel, IRequestFocus, IDialogViewModel, IDialogHostViewModel
    {
        /// <summary>
        /// Gets or sets all scales from data store
        /// </summary>
        public ICollection<Scale> AllScales { get; set; }

        private Scale newScale;

        /// <summary>
        /// Gets or sets a new <see cref="Scale"/> which user inputs
        /// </summary>
        public Scale NewScale
        {
            get
            {
                return newScale;
            }
            set
            {
                newScale = value;
                NotifyPropertyChanged(nameof(NewScale));
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NewScaleDialogViewModel"/> class
        /// </summary>
        /// <param name="allScales">A list of all <see cref="Scale"/> from the data store</param>
        /// <param name="dialogHostViewModel">A <see cref="IDialogHostViewModel"/> from which the <see cref="NewScaleDialogViewModel"/> is opened</param>
        public NewScaleDialogViewModel(ICollection<Scale> allScales, IDialogHostViewModel dialogHostViewModel)
        {
            NewScale = new Scale();
            AllScales = new ObservableCollection<Scale>(allScales);

            Ranges = new ObservableCollection<ScaleRange>();

            MessageQueue = new SnackbarMessageQueue();

            DialogHostViewModel = dialogHostViewModel;
        }

        #region IDialogViewModel Members

        public IDialogHostViewModel DialogHostViewModel { get; set; }

        public ICommand ConfirmCommand
        {
            get
            {
                return new ActionCommand(a => ConfirmDialog(), p => NewScale.IsValid);
            }
        }

        public void ConfirmDialog()
        {
            var scale = AllScales.SingleOrDefault(current => current.SerialNumber == NewScale.SerialNumber);
            if (scale != null)
            {
                MessageQueue.Enqueue("Serijski broj je zauzet");
                OnFocusRequested(nameof(NewScale.SerialNumber));

                return;
            }

            if (Ranges.Count == 0)
            {
                MessageQueue.Enqueue("Morate uneti bar 1 opseg");

                return;
            }
            else if (Ranges.Count >= 5)
            {
                MessageQueue.Enqueue("Ne možete uneti više od 5 opsega");

                return;
            }

            DialogHostViewModel.MessageQueue.Enqueue("Uspešno ste dodali novu vagu");
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

        #region Range Members

        private ICollection<ScaleRange> ranges;

        /// <summary>
        /// Gets or sets a list of <see cref="ScaleRange"/> for the <see cref="NewScale"/>
        /// </summary>
        public ICollection<ScaleRange> Ranges
        {
            get
            {
                return ranges;
            }
            set
            {
                ranges = value;
                NotifyPropertyChanged(nameof(Ranges));
            }
        }

        private ScaleRange selectedRange;

        /// <summary>
        /// Gets or sets the selected <see cref="ScaleRange"/> from the <see cref="Ranges"/>
        /// </summary>
        public ScaleRange SelectedRange
        {
            get
            {
                return selectedRange;
            }
            set
            {
                selectedRange = value;
                NotifyPropertyChanged(nameof(SelectedRange));
            }
        }

        /// <summary>
        /// Gets an <see cref="ICommand"/> for opening a <see cref="Views.Main.NewScaleRangeDialog"/>
        /// </summary>
        public ICommand ShowNewScaleRangeDialogCommad
        {
            get
            {
                return new ActionCommand(a => ShowNewScaleRangeDialog());
            }
        }

        /// <summary>
        /// Shows a <see cref="Views.Main.NewScaleRangeDialog"/>
        /// </summary>
        private void ShowNewScaleRangeDialog()
        {
            DialogViewModel = new NewScaleRangeDialogViewModel(NewScale.Ranges, this);

            IsDialogOpened = true;
        }

        /// <summary>
        /// Gets an <see cref="ICommand"/> for deleting the <see cref="SelectedRange"/>
        /// </summary>
        public ICommand DeleteScaleRangeCommand
        {
            get
            {
                return new ActionCommand(a => DeleteScaleRange(), p => SelectedRange != null);
            }
        }

        /// <summary>
        /// Removes the<see cref="SelectedRange"/> from the <see cref="Ranges"/>
        /// </summary>
        private void DeleteScaleRange()
        {
            NewScale.Ranges.Remove(SelectedRange);
            Ranges.Remove(SelectedRange);
        }

        #endregion

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

        #region IDialogHostViewModel

        private bool isDialogOpened;

        /// <summary>
        /// Gets or sets an indicator if the <see cref="Views.Main.NewScaleRangeDialog"/> is opened
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
                NotifyPropertyChanged(nameof(IsDialogOpened));

                if (!isDialogOpened && DialogViewModel != null)
                {
                    if (DialogViewModel.DialogResult.HasValue && DialogViewModel.DialogResult == true)
                    {
                        NewScale.Ranges.Add((DialogViewModel as NewScaleRangeDialogViewModel).NewScaleRange);
                        Ranges.Add((DialogViewModel as NewScaleRangeDialogViewModel).NewScaleRange);
                    }

                    DialogViewModel = null;
                }
            }
        }

        private IDialogViewModel dialogViewModel;

        /// <summary>
        /// Gets or sets a data context for the <see cref="Views.Main.NewScaleRangeDialog"/>
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
                NotifyPropertyChanged(nameof(DialogViewModel));
            }
        }

        /// <summary>
        /// Gets or sets a queue of messeges for the <see cref="Snackbar"/>
        /// </summary>
        public SnackbarMessageQueue MessageQueue { get; set; }

        #endregion
    }
}
