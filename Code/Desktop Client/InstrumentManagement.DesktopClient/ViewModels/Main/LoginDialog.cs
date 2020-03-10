namespace InstrumentManagement.DesktopClient.ViewModels.Main
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using InstrumentManagement.Windows;
    using MaterialDesignThemes.Wpf;
    using System.Collections.Generic;
    using System.Linq;
    using InstrumentManagement.Data.Accounts;
    using InstrumentManagement.Windows.FocusHandler;
    using InstrumentManagement.Windows.DialogHandler;

    /// <summary>
    /// A <see cref="ViewModel"/> containing all functionalities for the <see cref="Views.Main.LoginDialog"/>
    /// </summary>
    public class LoginDialogViewModel : ViewModel, IRequestFocus, IDialogViewModel
    {
        /// <summary>
        /// Gets or sets a list of all <see cref="Account"/> from data store
        /// </summary>
        public ICollection<Account> Accounts { get; set; }

        /// <summary>
        /// Gets or sets an <see cref="Account"/> that is selected from the <see cref="Accounts"/>
        /// </summary>
        public Account SelectedAccount { get; set; }

        /// <summary>
        /// Gets or sets a password that is inputed
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a message queue for the <see cref="Snackbar"/>
        /// </summary>
        public SnackbarMessageQueue MessageQueue { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="LoginDialogViewModel"/> class
        /// </summary>
        /// <param name="accounts">A list of all <see cref="Account"/> from the data store</param>
        /// <param name="dialogHostViewModel">An <see cref="IDialogHostViewModel"/> from which the <see cref="LoginDialogViewModel"/> is opened</param>
        public LoginDialogViewModel(ICollection<Account> accounts, IDialogHostViewModel dialogHostViewModel)
        {
            Accounts = new ObservableCollection<Account>(accounts);
            SelectedAccount = Accounts.First();

            DialogHostViewModel = dialogHostViewModel;

            MessageQueue = new SnackbarMessageQueue();
        }

        #region IDialogViewModel Members

        /// <summary>
        /// An <see cref="IDialogHostViewModel"/> from which the <see cref="LoginDialogViewModel"/> is opened
        /// </summary>
        public IDialogHostViewModel DialogHostViewModel { get; set; }

        /// <summary>
        /// Gets an <see cref="ICommand"/> for confirming the dialog
        /// </summary>
        public ICommand LoginCommand
        {
            get
            {
                return new ActionCommand(a => Login(), p => CanLogin);
            }
        }

        /// <summary>
        /// Checks if all values are valid for activating the <see cref="LoginCommand"/>
        /// </summary>
        public bool CanLogin
        {
            get
            {
                return SelectedAccount != null && !string.IsNullOrWhiteSpace(Password);
            }
        }

        /// <summary>
        /// Checks if password is correct and logs in
        /// </summary>
        public void Login()
        {
            if (SelectedAccount.Password == Password)
            {
                DialogResult = true;
                DialogHostViewModel.MessageQueue.Enqueue("Uspešno ste se ulogovali");
            }
            else
            {
                MessageQueue.Enqueue("Uneli ste pogrešnu lozinku");

                OnFocusRequested(nameof(Password));
            }
        }

        /// <summary>
        /// Gets an <see cref="ICommand"/> for canceling the dialog
        /// </summary>
        public ICommand CancelCommand
        {
            get
            {
                return new ActionCommand(a => DialogResult = false);
            }
        }

        private bool? dialogResult;

        /// <summary>
        /// Gets or sets a dialog result and closes the dialog
        /// </summary>
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
    }
}
