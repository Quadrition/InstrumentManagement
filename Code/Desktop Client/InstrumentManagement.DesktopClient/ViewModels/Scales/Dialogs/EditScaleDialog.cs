namespace InstrumentManagement.DesktopClient.ViewModels.Scales.Dialogs
{
    using InstrumentManagement.Data.Scales;
    using InstrumentManagement.Windows;
    using InstrumentManagement.Windows.DialogHandler;
    using System.ComponentModel.DataAnnotations;
    using System.Windows.Input;

    /// <summary>
    /// A <see cref="ViewModel"/> containing all functionalities for <see cref="Views.Scales.EditScaleDialog"/>
    /// </summary>
    public class EditScaleDialogViewModel : ViewModel, IDialogViewModel
    {
        private Scale scale;

        /// <summary>
        /// Gets or sets a <see cref="Data.Scales.Scale"/> which needs to be edited
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
        /// Initializes a new instance of <see cref="EditScaleDialogViewModel"/> class
        /// </summary>
        /// <param name="baseViewModel">A <see cref="ViewModel"/> from which is the <see cref="Views.Scales.EditScaleDialog"/> is opened</param>
        public EditScaleDialogViewModel(Scale scale, IDialogHostViewModel baseViewModel)
        {
            Scale = scale;

            NewInventoryNumber = Scale.InventoryNumber;
            NewLocation = Scale.Location;
            NewOrganizationalUnit = Scale.OrganizationalUnit;

            DialogHostViewModel = baseViewModel;
        }

        #region New Values Members

        private string newInventoryNumber;

        /// <summary>
        /// Gets or sets a new value for the inventory number
        /// </summary>
        [StringLength(32, ErrorMessage = "Inventarni broj može imati najviše 32 karaktera")]
        public string NewInventoryNumber
        {
            get
            {
                return newInventoryNumber;
            }
            set
            {
                newInventoryNumber = value;
                NotifyPropertyChanged(nameof(NewInventoryNumber));
            }
        }

        private string newLocation;

        /// <summary>
        /// Gets or sets a new value for the location
        /// </summary>
        [StringLength(32, ErrorMessage = "Lokacija može imati najviše 32 karaktera")]
        public string NewLocation
        {
            get
            {
                return newLocation;
            }
            set
            {
                newLocation = value;
                NotifyPropertyChanged(nameof(NewLocation));
            }
        }

        private string newOrganizationalUnit;

        /// <summary>
        /// Gets or set a new value for the organizational unit
        /// </summary>
        [StringLength(32, ErrorMessage = "Organizaciona jedinica može imati najviše 32 karaktera")]
        public string NewOrganizationalUnit
        {
            get
            {
                return newOrganizationalUnit;
            }
            set
            {
                newOrganizationalUnit = value;
                NotifyPropertyChanged(nameof(NewOrganizationalUnit));
            }
        }

        #endregion

        #region IDialogViewModel Members

        public IDialogHostViewModel DialogHostViewModel { get; set; }

        public ICommand ConfirmCommand
        {
            get
            {
                return new ActionCommand(a => ConfirmDialog(), p => CanEdit);
            }
        }

        /// <summary>
        /// Checks if the <see cref="Scale"/> can be edited
        /// </summary>
        private bool CanEdit
        {
            get
            {
                string[] properties = { "NewInventoryNumber", "NewLocation", "NewOrganizationalUnit" };

                foreach (string property in properties)
                {
                    if (OnValidate(property) != null)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public void ConfirmDialog()
        {
            Scale.InventoryNumber = NewInventoryNumber;
            Scale.Location = NewLocation;
            Scale.OrganizationalUnit = NewOrganizationalUnit;

            DialogResult = true;
            DialogHostViewModel.MessageQueue.Enqueue("Uspešno ste izmenili vagu");
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
