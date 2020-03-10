namespace InstrumentManagement.DesktopClient.ViewModels.Administration
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using InstrumentManagement.Data.Accounts;
    using InstrumentManagement.Windows;
    using InstrumentManagement.Windows.DialogHandler;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// A <see cref="ViewModel"/> containing all functionalities for the <see cref="Views.Administration.NewAccountDialog"/>
    /// </summary>
    public class NewAccountDialogViewModel : ViewModel, IDialogViewModel
    {
        /// <summary>
        /// Gets or sets a list of all <see cref="Account"/> from the data store
        /// </summary>
        public ICollection<Account> Accounts { get; set; }

        /// <summary>
        /// Gets or sets an <see cref="Account"/> that is currently being inputed
        /// </summary>
        public Account NewAccount { get; set; }

        /// <summary>
        /// Gets or sets a message queue for the <see cref="MaterialDesignThemes.Wpf.Snackbar"/>
        /// </summary>
        public SnackbarMessageQueue MessageQueue { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="NewAccountDialogViewModel"/> class
        /// </summary>
        /// <param name="accounts">A list of all <see cref="Account"/> from the data store</param>
        /// <param name="dialogHostViewModel">An <see cref="IDialogHostViewModel"/> from which the <see cref="NewAccountDialogViewModel"/> is opened</param>
        public NewAccountDialogViewModel(ICollection<Account> accounts, IDialogHostViewModel dialogHostViewModel)
        {
            Accounts = accounts;
            NewAccount = new User();

            DialogHostViewModel = dialogHostViewModel;

            MessageQueue = new SnackbarMessageQueue();
        }

        #region IDialogViewModel Members

        /// <summary>
        /// An <see cref="IDialogHostViewModel"/> from which the <see cref="NewAccountDialogViewModel"/> is opened
        /// </summary>
        public IDialogHostViewModel DialogHostViewModel { get; set; }

        /// <summary>
        /// Gets an <see cref="ICommand"/> for confirming the dialog
        /// </summary>
        public ICommand CreateAccountCommand
        {
            get
            {
                return new ActionCommand(a => ConfirmDialog(), p => NewAccount.IsValid);
            }
        }

        /// <summary>
        /// Checks if the <see cref="NewAccount"/> exists in the data store and saves changes
        /// </summary>
        public void ConfirmDialog()
        {
            Account account = Accounts.SingleOrDefault(acc => acc.Username == NewAccount.Username);
            if (account != null)
            {
                MessageQueue.Enqueue("Korisničko ime je zauzeto");

                return;
            }

            DialogHostViewModel.MessageQueue.Enqueue("Uspešno ste uneli novog korisnika");
            DialogResult = true;
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
    }
}
