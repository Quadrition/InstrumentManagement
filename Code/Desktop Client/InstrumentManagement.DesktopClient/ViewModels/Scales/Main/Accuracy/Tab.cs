namespace InstrumentManagement.DesktopClient.ViewModels.Scales.Main
{
    using InstrumentManagement.Windows;
    using System.Windows.Input;
    using InstrumentManagement.Data.Scales.Accuracy;
    using InstrumentManagement.DesktopClient.ViewModels.Scales.Dialogs;

    public partial class ScaleWindowViewModel
    {
        /// <summary>
        /// Gets an <see cref="ICommand"/> for showing a scale accuracy
        /// </summary>
        public ICommand ShowAccuracyCommand
        {
            get
            {
                return new ActionCommand(a => TransitionerSelectedIndex = 2, p => SelectedCalibration != null && SelectedCalibration.Repeatability.ReferenceValue != null);
            }
        }

        private int transitionerAccuracySelectedIndex;

        /// <summary>
        /// Gets or sets a selected index of <see cref="MaterialDesignThemes.Wpf.Transitions.Transitioner"/> in accuracy
        /// </summary>
        public int TransitionerAccuracySelectedIndex
        {
            get
            {
                return transitionerAccuracySelectedIndex;
            }
            set
            {
                transitionerAccuracySelectedIndex = value;
                NotifyPropertyChanged(nameof(TransitionerAccuracySelectedIndex));

                //TODO popups close
            }
        }

        /// <summary>
        /// Gets an <see cref="ICommand"/> for opening a <see cref="Views.Scales.Dialogs.NewAccuracyReferenceValueDialog"/>
        /// </summary>
        public ICommand ShowNewScaleAccuracyReferenceValueDialogCommand
        {
            get
            {
                return new ActionCommand(a => ShowNewScaleAccuracyReferenceValueDialog(), p => IsLastCalibration == true);
            }
        }

        /// <summary>
        /// Opens a <see cref="Views.Scales.Dialogs.NewAccuracyReferenceValueDialog"/>
        /// </summary>
        private void ShowNewScaleAccuracyReferenceValueDialog()
        {
            DialogViewModel = new NewAccuracyReferenceValueDialogViewModel(SelectedCalibration.Accuracy, this);

            DialogContent = new Views.Scales.Dialogs.NewAccuracyReferenceValueDialog()
            {
                DataContext = DialogViewModel
            };

            IsDialogOpened = true;
        }

        #region Side Menu Commands

        /// <summary>
        /// Gets an <see cref="ICommand"/> for showing a <see cref="ScaleAccuracyReferenceValue"/>
        /// </summary>
        public ICommand ShowAccuracyReferenceValueCommand
        {
            get
            {
                return new ActionCommand(a => TransitionerAccuracySelectedIndex = 1, p => TransitionerAccuracySelectedIndex != 0);
            }
        }

        /// <summary>
        /// Gets an <see cref="ICommand"/> for showing a all <see cref="ScaleAccuracyTest"/>
        /// </summary>
        public ICommand ShowAccuracyTestsCommand
        {
            get
            {
                return new ActionCommand(a => TransitionerAccuracySelectedIndex = 2, p => TransitionerAccuracySelectedIndex != 0);
            }
        }

        /// <summary>
        /// Gets an <see cref="ICommand"/> for showing a all <see cref="ScaleAccuracyTest"/> chart
        /// </summary>
        public ICommand ShowAccuracyChartCommand
        {
            get
            {
                return new ActionCommand(a => TransitionerAccuracySelectedIndex = 3, p => TransitionerAccuracySelectedIndex != 0);
            }
        }

        #endregion
    }
}
