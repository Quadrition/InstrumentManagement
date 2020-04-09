namespace InstrumentManagement.DesktopClient.ViewModels.Scales.Main
{
    using InstrumentManagement.Windows;
    using System.Windows.Input;
    using InstrumentManagement.Data.Scales.Repeatability;
    using InstrumentManagement.DesktopClient.ViewModels.Scales.Dialogs;

    public partial class ScaleWindowViewModel
    {
        /// <summary>
        /// Gets an <see cref="ICommand"/> for showing a <see cref="ScaleRepeatabilityReferenceValue"/>
        /// </summary>
        public ICommand ShowRepeatabilityCommand
        {
            get
            {
                return new ActionCommand(a => TransitionerSelectedIndex = 1, p => SelectedCalibration != null);
            }
        }

        private int transitionerRepeatabilitySelectedIndex;

        /// <summary>
        /// Gets or sets a selected index of <see cref="MaterialDesignThemes.Wpf.Transitions.Transitioner"/> in repeatability
        /// </summary>
        public int TransitionerRepeatabilitySelectedIndex
        {
            get
            {
                return transitionerRepeatabilitySelectedIndex;
            }
            set
            {
                transitionerRepeatabilitySelectedIndex = value;
                NotifyPropertyChanged(nameof(TransitionerRepeatabilitySelectedIndex));

                IsPopupRepeatabilityDataGridPrintingOpen = false;
            }
        }

        #region Side Menu Commands

        /// <summary>
        /// Gets an <see cref="ICommand"/> for showing a <see cref="ScaleRepeatabilityReferenceValue"/>
        /// </summary>
        public ICommand ShowRepeatabilityReferenceValueCommand
        {
            get
            {
                return new ActionCommand(a => TransitionerRepeatabilitySelectedIndex = 1, p => TransitionerRepeatabilitySelectedIndex != 0);
            }
        }

        /// <summary>
        /// Gets an <see cref="ICommand"/> for showing a all <see cref="ScaleRepeatabilityTest"/>
        /// </summary>
        public ICommand ShowRepeatabilityTestsCommand
        {
            get
            {
                return new ActionCommand(a => TransitionerRepeatabilitySelectedIndex = 2, p => TransitionerRepeatabilitySelectedIndex != 0);
            }
        }

        /// <summary>
        /// Gets an <see cref="ICommand"/> for showing a all <see cref="ScaleRepeatabilityTest"/>
        /// </summary>
        public ICommand ShowRepeatabilityChartCommand
        {
            get
            {
                return new ActionCommand(a => TransitionerRepeatabilitySelectedIndex = 3, p => TransitionerRepeatabilitySelectedIndex != 0);
            }
        }

        #endregion 

        /// <summary>
        /// Gets an <see cref="ICommand"/> for opening a <see cref="Views.Scales.Dialogs.NewRepeatabilityReferenceValueDialog"/>
        /// </summary>
        public ICommand ShowNewScaleRepeatabilityReferenceValueDialogCommand
        {
            get
            {
                return new ActionCommand(a => ShowNewScaleRepeatabilityReferenceValueDialog(), p => IsLastCalibration == true);
            }
        }

        /// <summary>
        /// Opens a <see cref="Views.Scales.Dialogs.NewRepeatabilityReferenceValueDialog"/>
        /// </summary>
        private void ShowNewScaleRepeatabilityReferenceValueDialog()
        {
            DialogViewModel = new NewRepeatabilityReferenceValueDialogViewModel(SelectedCalibration.Repeatability, this);

            DialogContent = new Views.Scales.Dialogs.NewRepeatabilityReferenceValueDialog()
            {
                DataContext = DialogViewModel
            };

            IsDialogOpened = true;
        }
    }
}
