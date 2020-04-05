namespace InstrumentManagement.DesktopClient.ViewModels.Administration
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using InstrumentManagement.Data;
    using InstrumentManagement.Data.Accounts;
    using InstrumentManagement.Data.Scales;
    using InstrumentManagement.Windows;
    using InstrumentManagement.Windows.ClearHandler;
    using InstrumentManagement.Windows.DialogHandler;
    using InstrumentManagement.Windows.FocusHandler;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// A <see cref="ViewModel"/> containing all functionalities for the <see cref="Views.Administration.AdministrationDialog"/>
    /// </summary>
    public class AdministrationDialogViewModel : ViewModel, IRequestFocus, IRequestClear, IDialogHostViewModel, IDialogViewModel
    {
        private readonly BusinessContext context;

        /// <summary>
        /// Gets or sets a list of all <see cref="Account"/> from data store
        /// </summary>
        public ICollection<Account> Accounts { get; set; }

        private Account selectedAccount;

        /// <summary>
        /// Gets or sets an <see cref="Account"/> that is selected
        /// </summary>
        public Account SelectedAccount
        {
            get
            {
                return selectedAccount;
            }
            set
            {
                selectedAccount = value;
                NotifyPropertyChanged(nameof(SelectedAccount));

                IsFlipped = false;

                switch (value)
                {
                    case User user:
                        UnallowedScales = new ObservableCollection<Scale>(context.GetAllScales().Except(user.Scales));
                        AllowedScales = new ObservableCollection<Scale>(user.Scales);
                        break;

                    case Administrator administration:
                        UnallowedScales = null;
                        AllowedScales = null;
                        IsExpanded = false;
                        break;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="AdministrationDialogViewModel"/> class
        /// </summary>
        /// <param name="context">A <see cref="BusinessContext"/> layer</param>
        /// <param name="dialogHostViewModel">An <see cref="IDialogHostViewModel"/> from which the <see cref="AdministrationDialogViewModel"/> is opened</param>
        public AdministrationDialogViewModel(BusinessContext context, IDialogHostViewModel dialogHostViewModel)
        {
            this.context = context;

            MessageQueue = new SnackbarMessageQueue();

            DialogHostViewModel = dialogHostViewModel;

            Accounts = new ObservableCollection<Account>(context.GetAccounts());
            SelectedAccount = Accounts.First();
        }

        #region IDialogHostViewModel Members

        private bool isDialogOpened;

        /// <summary>
        /// Gets or sets an indicator if the dialog is opened
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
                    if (DialogViewModel.DialogResult == true)
                    {
                        switch (DialogViewModel)
                        {
                            case NewAccountDialogViewModel newAccountDialogViewModel:
                                this.context.AddNewAccount(newAccountDialogViewModel.NewAccount);
                                Accounts.Add(newAccountDialogViewModel.NewAccount);
                                break;
                        }
                    }

                    DialogViewModel = null;
                }
            }
        }

        private IDialogViewModel dialogViewModel;

        /// <summary>
        /// Gets or sets a datacontext for the dialog content
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
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a message queue for the <see cref="MaterialDesignThemes.Wpf.Snackbar"/>
        /// </summary>
        public SnackbarMessageQueue MessageQueue { get; set; }

        #endregion

        #region New User Members

        /// <summary>
        /// Gets an <see cref="ICommand"/> for opening a <see cref="Views.Administration.NewAccountDialog"/>
        /// </summary>
        public ICommand NewAccountCommand
        {
            get
            {
                return new ActionCommand(a => OpenNewAccountDialog());
            }
        }

        /// <summary>
        /// Opens a <see cref="Views.Administration.NewAccountDialog"/>
        /// </summary>
        private void OpenNewAccountDialog()
        {
            DialogViewModel = new NewAccountDialogViewModel(context.GetAccounts(), this);

            IsDialogOpened = true;
        }

        #endregion

        #region Change Password Members

        private bool isFlipped;

        /// <summary>
        /// Indicates if the <see cref="Flipper"/> is flipped
        /// </summary>
        public bool IsFlipped
        {
            get
            {
                return isFlipped;
            }
            set
            {
                isFlipped = value;
                NotifyPropertyChanged(nameof(IsFlipped));

                if (value == false)
                {
                    OnClearRequested(nameof(OldPassword));
                    OnClearRequested(nameof(NewPassword));
                    OnClearRequested(nameof(ConfirmedNewPassword));
                }
            }
        }

        /// <summary>
        /// Gets or sets an old password that user has inputed
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// Gets or sets a new password that user has inputed
        /// </summary>
        public string NewPassword { get; set; }

        /// <summary>
        /// Gets or sets a confirmation of a new password that user has inputed
        /// </summary>
        public string ConfirmedNewPassword { get; set; }

        /// <summary>
        /// Gets an <see cref="ICommand"/> for changing a password of <see cref="SelectedAccount"/>
        /// </summary>
        public ICommand ChangePasswordCommand
        {
            get
            {
                return new ActionCommand(a => ChangePassword(), p => SelectedAccount != null);
            }
        }

        /// <summary>
        /// Checks if entered passwords are valid and changes the <see cref="SelectedAccount"/> password
        /// </summary>
        private void ChangePassword()
        {
            if (string.IsNullOrWhiteSpace(OldPassword))
            {
                MessageQueue.Enqueue("Stara lozinka je obavezna i ne može biti prazna");
                OnFocusRequested(nameof(OldPassword));

                return;
            }

            if (string.IsNullOrWhiteSpace(NewPassword))
            {
                MessageQueue.Enqueue("Nova lozinka je obavezna i ne može biti prazna");
                OnFocusRequested(nameof(NewPassword));

                return;
            }

            if (string.IsNullOrWhiteSpace(ConfirmedNewPassword))
            {
                MessageQueue.Enqueue("Potvrda lozinke je obavezna i ne može biti prazna");
                OnFocusRequested(nameof(ConfirmedNewPassword));

                return;
            }

            if (SelectedAccount.Password != OldPassword)
            {
                MessageQueue.Enqueue("Stara lozinka je pogrešna");
                OnFocusRequested(nameof(OldPassword));

                return;
            }

            if (NewPassword != ConfirmedNewPassword)
            {
                MessageQueue.Enqueue("Nove lozinke se ne poklapaju");
                OnFocusRequested(nameof(ConfirmedNewPassword));

                return;
            }

            if (NewPassword == OldPassword)
            {
                MessageQueue.Enqueue("Nova i stara lozinka ne mogu biti iste");
                OnFocusRequested(nameof(NewPassword));

                return;
            }

            SelectedAccount.Password = NewPassword;
            this.context.UpdateAccount(SelectedAccount);

            IsFlipped = false;

            MessageQueue.Enqueue("Lozinka je uspešno promenjena");
        }

        #endregion

        #region Accesses Members

        private bool isExpanded;

        /// <summary>
        /// Gets or sets if the <see cref="System.Windows.Controls.Expander"/> for the accesses is expanded
        /// </summary>
        public bool IsExpanded
        {
            get
            {
                return isExpanded;
            }
            set
            {
                if (value == true && SelectedAccount is Administrator)
                {
                    MessageQueue.Enqueue("Administrator ima pristup svim vagama");
                }
                else
                {
                    isExpanded = value;
                    NotifyPropertyChanged(nameof(IsExpanded));
                }
            }
        }

        private Scale selectedAllowedScale;

        /// <summary>
        /// Gets or sets a selected <see cref="Scale"/> from the <see cref="AllowedScales"/>
        /// </summary>
        public Scale SelectedAllowedScale
        {
            get
            {
                return selectedAllowedScale;
            }
            set
            {
                selectedAllowedScale = value;
                NotifyPropertyChanged(nameof(SelectedAllowedScale));
            }
        }

        private Scale selectedUnallowedScale;

        /// <summary>
        /// Gets or sets a selected <see cref="Scale"/> from the <see cref="UnallowedScales"/>
        /// </summary>
        public Scale SelectedUnallowedScale
        {
            get
            {
                return selectedUnallowedScale;
            }
            set
            {
                selectedUnallowedScale = value;
                NotifyPropertyChanged(nameof(SelectedUnallowedScale));
            }
        }

        private ICollection<Scale> unallowedScales;

        /// <summary>
        /// Gets or sets a list of <see cref="Scale"/> that are unallowed to the <see cref="SelectedAccount"/>
        /// </summary>
        public ICollection<Scale> UnallowedScales
        {
            get
            {
                return unallowedScales;
            }
            set
            {
                unallowedScales = value;
                NotifyPropertyChanged(nameof(UnallowedScales));
            }
        }

        private ICollection<Scale> allowedScales;

        /// <summary>
        /// Gets or sets a list of <see cref="Scale"/> that are allowed to the <see cref="SelectedAccount"/>
        /// </summary>
        public ICollection<Scale> AllowedScales
        {
            get
            {
                return allowedScales;
            }
            set
            {
                allowedScales = value;
                NotifyPropertyChanged(nameof(AllowedScales));
            }
        }

        /// <summary>
        /// Gets an <see cref="ICommand"/> for adding an access for the <see cref="User"/> to the <see cref="SelectedUnallowedScale"/>
        /// </summary>
        public ICommand AddScaleAccessCommand
        {
            get
            {
                return new ActionCommand(a => AddScaleAccess(), p => SelectedUnallowedScale != null && SelectedAccount is User);
            }
        }

        /// <summary>
        /// Adds an acccess of the <see cref="SelectedUnallowedScale"/> to the <see cref="SelectedAccount"/>
        /// </summary>
        private void AddScaleAccess()
        {
            AllowedScales.Add(SelectedUnallowedScale);

            (SelectedAccount as User).Scales.Add(SelectedUnallowedScale);
            context.UpdateAccount(SelectedAccount);

            UnallowedScales.Remove(SelectedUnallowedScale);
        }

        /// <summary>
        /// Gets an <see cref="ICommand"/> for removing an access for the <see cref="User"/> to the <see cref="SelectedAllowedScale"/>
        /// </summary>
        public ICommand RemoveScaleAccessCommand
        {
            get
            {
                return new ActionCommand(a => RemoveScaleAccess(), p => SelectedAllowedScale != null && SelectedAccount is User);
            }
        }

        /// <summary>
        /// Removes an acccess of the <see cref="SelectedAllowedScale"/> to the <see cref="User"/>
        /// </summary>
        private void RemoveScaleAccess()
        {
            UnallowedScales.Add(SelectedAllowedScale);

            (SelectedAccount as User).Scales.Remove(SelectedAllowedScale);
            context.UpdateAccount(SelectedAccount);

            AllowedScales.Remove(SelectedAllowedScale);
        }

        #endregion

        #region IRequestFocus Members

        public event EventHandler<FocusRequestedEventArgs> FocusRequested;

        /// <summary>
        /// Requesting a component from a View to be focused
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnFocusRequested(string propertyName)
        {
            FocusRequested?.Invoke(this, new FocusRequestedEventArgs(propertyName));
        }

        #endregion

        #region IRequestedClear Members

        public event EventHandler<ClearRequestedEventArgs> ClearRequested;

        /// <summary>
        /// Requesting a component from a View to be cleared
        /// </summary>
        /// <param name="propertyName">A name of a property to be cleared</param>
        protected virtual void OnClearRequested(string propertyName)
        {
            ClearRequested?.Invoke(this, new ClearRequestedEventArgs(propertyName));
        }

        #endregion

        #region IDialogViewModel

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

        /// <summary>
        /// Gets an <see cref="ICommand"/> for closing the dialog
        /// </summary>
        public ICommand CloseCommand
        {
            get
            {
                return new ActionCommand(a => DialogResult = false);
            }
        }

        #endregion
    }
}
