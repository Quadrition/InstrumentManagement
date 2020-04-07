namespace InstrumentManagement.DesktopClient.ViewModels.Scales.Main
{
    using InstrumentManagement.Windows;
    using System.Windows.Input;
    using InstrumentManagement.Data.Scales.Repeatability;

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

                //IsPopupRepeatabilityDataGridPrintingOpen = false;
                //TODO add pop up remove
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
    }
}
