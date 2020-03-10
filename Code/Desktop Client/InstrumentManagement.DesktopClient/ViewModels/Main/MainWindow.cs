namespace InstrumentManagement.DesktopClient.ViewModels.Main
{
    using InstrumentManagement.Data;
    using InstrumentManagement.Data.Accounts;
    using InstrumentManagement.Data.Scales;
    using InstrumentManagement.DesktopClient.ViewModels.Administration;
    using InstrumentManagement.Windows;
    using InstrumentManagement.Windows.DialogHandler;
    using MaterialDesignThemes.Wpf;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// A <see cref="ViewModel"/> containing all functionalities for the <see cref="Views.Main.MainWindow"/>
    /// </summary>
    public partial class MainWindowViewModel : ViewModel, IDialogHostViewModel
    {
        private readonly BusinessContext context;

        private Visibility subMenuAndTransitionerVisibility;

        /// <summary>
        /// Gets or sets a <see cref="Visibility"/> of the menu
        /// </summary>
        public Visibility SubMenuAndTransitionerVisibility
        {
            get
            {
                return subMenuAndTransitionerVisibility;
            }
            set
            {
                subMenuAndTransitionerVisibility = value;
                NotifyPropertyChanged(nameof(SubMenuAndTransitionerVisibility));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class
        /// </summary>
        public MainWindowViewModel() : this(new BusinessContext())
        {

        }

        /// <summary>
        /// Initializes a new instance of <see cref="MainWindowViewModel"/> class
        /// </summary>
        /// <param name="context">A <see cref="BusinessContext"/> layer</param>
        public MainWindowViewModel(BusinessContext context)
        {
            this.context = context;

            ScalesDataGridRowDetailsVisibility = DataGridRowDetailsVisibilityMode.Collapsed;

            MessageQueue = new SnackbarMessageQueue();

            SubMenuAndTransitionerVisibility = Visibility.Collapsed;

            //TODO skloni ovo
            if (context.GetAccounts().Count == 0)
            {
                context.AddNewAccount(new Administrator()
                {
                    Username = "Admin",
                    Password = "admin"
                });
            }
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
                            case LoginDialogViewModel loginDialogViewModel:
                                LoggedAccount = loginDialogViewModel.SelectedAccount;
                                break;

                            case EditInfoDialogViewModel editMainWindowInfoDialogViewModel:
                                Info = editMainWindowInfoDialogViewModel.Info;
                                break;
                        }
                    }

                    DialogViewModel = null;
                    DialogContent = null;
                }
            }
        }

        private UserControl dialogContent;

        /// <summary>
        /// Gets or sets a content of the dialog
        /// </summary>
        public UserControl DialogContent
        {
            get
            {
                return dialogContent;
            }
            set
            {
                dialogContent = value;
                NotifyPropertyChanged(nameof(DialogContent));
            }
        }

        private IDialogViewModel dialogViewModel;

        /// <summary>
        /// Gets or sets a data context for the <see cref="DialogContent"/>
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
        /// Gets or sets a message queue for the <see cref="Snackbar"/>
        /// </summary>
        public SnackbarMessageQueue MessageQueue { get; set; }

        #endregion

        #region SideMenu Members

        /// <summary>
        /// Gets an <see cref="ICommand"/> for showing a tab for the <see cref="Scales"/>
        /// </summary>
        public ICommand ShowScalesCommand
        {
            get
            {
                return new ActionCommand(a => TransitionerSelectedIndex = 0);
            }
        }

        private int transitionerSelectedIndex;

        /// <summary>
        /// Gets or sets an index of the transitioner
        /// </summary>
        public int TransitionerSelectedIndex
        {
            get
            {
                return transitionerSelectedIndex;
            }
            set
            {
                transitionerSelectedIndex = value;
                NotifyPropertyChanged(nameof(TransitionerSelectedIndex));
            }
        }

        #endregion

        #region Licence Members

        private DateTime expirationDate;

        /// <summary>
        /// Gets or sets an expiration date from the licence
        /// </summary>
        public DateTime ExpirationDate
        {
            get
            {
                return expirationDate;
            }
            set
            {
                expirationDate = value;
                NotifyPropertyChanged(nameof(ExpirationDate));
            }
        }

        private int maxScaleCount;

        /// <summary>
        /// Gets or sets a maximum <see cref="Scales"/> that can be inputed from the licence 
        /// </summary>
        public int MaxScaleCount
        {
            get
            {
                return maxScaleCount;
            }
            set
            {
                maxScaleCount = value;
                NotifyPropertyChanged(nameof(MaxScaleCount));
            }
        }

        #endregion

        #region Edit Info Members

        private string info;

        /// <summary>
        /// Gets or sets a info message on the <see cref="Views.Main.MainWindow"/>
        /// </summary>
        public string Info
        {
            get
            {
                return info;
            }
            set
            {
                info = value;
                NotifyPropertyChanged(nameof(Info));
            }
        }

        /// <summary>
        /// Gets an <see cref="ICommand"/> for opening a <see cref="Views.Main.EditInfoDialog"/>
        /// </summary>
        public ICommand EditInfoCommand
        {
            get
            {
                return new ActionCommand(a => OpenEditInfoDialog(), p => LoggedAccount is Administrator);
            }
        }

        /// <summary>
        /// Opens a <see cref="Views.Main.EditInfoDialog"/>
        /// </summary>
        private void OpenEditInfoDialog()
        {
            DialogViewModel = new EditInfoDialogViewModel(Info, this);

            DialogContent = new Views.Main.EditInfoDialog()
            {
                DataContext = DialogViewModel
            };

            IsDialogOpened = true;
        }

        #endregion

        #region Login/Logout Commands

        #region Login Members

        /// <summary>
        /// Gets an <see cref="ICommand"/> for showing a <see cref="Views.Main.LoginDialog"/>
        /// </summary>
        public ICommand LoginCommand
        {
            get
            {
                return new ActionCommand(a => ShowLoginDialog(), p => LoggedAccount == null);
            }
        }

        /// <summary>
        /// Shows a <see cref="Views.Main.LoginDialog"/>
        /// </summary>
        private void ShowLoginDialog()
        {
            DialogViewModel = new LoginDialogViewModel(this.context.GetAccounts(), this);

            DialogContent = new Views.Main.LoginDialog()
            {
                DataContext = DialogViewModel
            };

            IsDialogOpened = true;
        }

        #endregion

        #region Logout Members

        /// <summary>
        /// Gets an <see cref="ICommand"/> for logging out an <see cref="LoggedAccount"/>
        /// </summary>
        public ICommand LogoutCommand
        {
            get
            {
                return new ActionCommand(a => Logout(), p => LoggedAccount != null);
            }
        }

        /// <summary>
        /// Logs out user and displays an info message
        /// </summary>
        private void Logout()
        {
            LoggedAccount = null;

            MessageQueue.Enqueue("Uspešno ste se izlogovali");
        }

        #endregion

        private Account loggedAccount;

        /// <summary>
        /// Represents an <see cref="Account"/> that is currently logged in
        /// </summary>
        public Account LoggedAccount
        {
            get
            {
                return loggedAccount;
            }
            set
            {
                loggedAccount = value;

                if (value == null)
                {
                    Scales = null;
                    SubMenuAndTransitionerVisibility = Visibility.Collapsed;
                }
                else
                {
                    SubMenuAndTransitionerVisibility = Visibility.Visible;

                    switch (value)
                    {
                        case Administrator administrator:
                            Scales = new ObservableCollection<Scale>(this.context.GetAllScales());
                            break;

                        case User user:
                            Scales = new ObservableCollection<Scale>(user.Scales);
                            break;
                    }

                    SelectedScale = Scales.FirstOrDefault();
                }

                NotifyPropertyChanged(nameof(LoggedAccount));
            }
        }

        #endregion

        #region Administration Commands

        /// <summary>
        /// Gets an <see cref="ICommand"/> for showing a <see cref="Views.Administration.AdministrationDialog"/>
        /// </summary>
        public ICommand ShowAdministrationDialogCommand
        {
            get
            {
                return new ActionCommand(a => ShowAdministrationDialog(), p => LoggedAccount is Administrator);
            }
        }

        /// <summary>
        /// Shows the <see cref="Views.Administration.AdministrationDialog"/>
        /// </summary>
        private void ShowAdministrationDialog()
        {
            DialogViewModel = new AdministrationDialogViewModel(this.context, this);

            DialogContent = new Views.Administration.AdministrationDialog()
            {
                DataContext = DialogViewModel
            };

            IsDialogOpened = true;
        }

        #endregion
    }
}
