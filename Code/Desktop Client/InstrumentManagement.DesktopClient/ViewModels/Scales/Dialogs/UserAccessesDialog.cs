namespace InstrumentManagement.DesktopClient.ViewModels.Scales.Dialogs
{
    using InstrumentManagement.Data;
    using InstrumentManagement.Data.Accounts;
    using InstrumentManagement.Data.Scales;
    using InstrumentManagement.Windows;
    using InstrumentManagement.Windows.DialogHandler;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    /// <summary>
    /// A <see cref="ViewModel"/> containing all functionalities for the <see cref="Views.Scales.Dialogs.UserAccessesDialog"/>
    /// </summary>
    public class UserAccessesDialogViewModel : ViewModel, IDialogViewModel
    {
        private BusinessContext context;

        private Scale scale;

        /// <summary>
        /// Gets or sets a <see cref="Scale"/> which accesses are being edited
        /// </summary>
        public Scale Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
                NotifyPropertyChanged(nameof(Scale));
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="UserAccessesDialogViewModel"/> class
        /// </summary>
        /// <param name="allUsers">A list of all <see cref="User"/> from the data store
        /// <param name="scale">A <see cref="Scale"/> which accesses needs to be edited
        /// <param name="dialogHostViewModel">An <see cref="IDialogHostViewModel"/> from which the <see cref="UserAccessesDialogViewModel"/> is opened</param>
        /// <param name="context">An <see cref="BusinessContext"/> layer</param>
        public UserAccessesDialogViewModel(ICollection<User> allUsers, Scale scale, IDialogHostViewModel dialogHostViewModel, BusinessContext context)
        {
            Scale = scale;
            UnallowedUsers = new ObservableCollection<User>(allUsers.Except(scale.Users));
            AllowedUsers = new ObservableCollection<User>(Scale.Users);

            this.context = context;

            DialogHostViewModel = dialogHostViewModel;
        }

        #region User Access Members

        private User selectedAllowedUser;

        /// <summary>
        /// Gets or sets a selected <see cref="User"/> that can access the <see cref="Scale"/>
        /// </summary>
        public User SelectedAllowedUser
        {
            get
            {
                return selectedAllowedUser;
            }
            set
            {
                selectedAllowedUser = value;
                NotifyPropertyChanged(nameof(SelectedAllowedUser));
            }
        }

        private User selectedUnallowedUser;

        /// <summary>
        /// Gets or sets a selected <see cref="User"/> that cannot access the <see cref="Scale"/>
        /// </summary>
        public User SelectedUnallowedUser
        {
            get
            {
                return selectedUnallowedUser;
            }
            set
            {
                selectedUnallowedUser = value;
                NotifyPropertyChanged(nameof(SelectedUnallowedUser));
            }
        }

        /// <summary>
        /// Gets or sets a list of <see cref="User"/> that cannot access the <see cref="Scale"/>
        /// </summary>
        public ICollection<User> UnallowedUsers { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="User"/> that can access the <see cref="Scale"/>
        /// </summary>
        public ICollection<User> AllowedUsers { get; set; }

        /// <summary>
        /// Gets a command for adding an access to the user
        /// </summary>
        public ICommand AddScaleAccessCommand
        {
            get
            {
                return new ActionCommand(a => AddScaleAccess(), p => SelectedUnallowedUser != null);
            }
        }

        /// <summary>
        /// Adds <see cref="SelectedUnallowedUser"/> an access to the <see cref="Scale"/>
        /// </summary>
        private void AddScaleAccess()
        {
            AllowedUsers.Add(SelectedUnallowedUser);

            Scale.Users.Add(SelectedUnallowedUser);
            context.UpdateScale(Scale);

            UnallowedUsers.Remove(SelectedUnallowedUser);
        }

        /// <summary>
        /// Gets a command for removing an access to the user
        /// </summary>
        public ICommand RemoveScaleAccessCommand
        {
            get
            {
                return new ActionCommand(a => RemoveScaleAccess(), p => SelectedAllowedUser != null);
            }
        }

        /// <summary>
        /// Removes <see cref="SelectedUnallowedUser"/> an access to the <see cref="Scale"/>
        /// </summary>
        private void RemoveScaleAccess()
        {
            UnallowedUsers.Add(SelectedAllowedUser);

            Scale.Users.Remove(SelectedAllowedUser);
            context.UpdateScale(Scale);

            AllowedUsers.Remove(SelectedAllowedUser);
        }

        #endregion

        #region IDialogViewModel Members

        public IDialogHostViewModel DialogHostViewModel { get; set; }

        public ICommand CloseCommand
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
