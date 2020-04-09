namespace InstrumentManagement.DesktopClient.ViewModels.Scales.Main
{
    using InstrumentManagement.Data;
    using InstrumentManagement.Data.Accounts;
    using InstrumentManagement.Data.Scales;
    using InstrumentManagement.Data.Scales.Calibration;
    using InstrumentManagement.Data.Scales.Repeatability;
    using InstrumentManagement.DesktopClient.ViewModels.Scales.Dialogs;
    using InstrumentManagement.Windows;
    using InstrumentManagement.Windows.DialogHandler;
    using MaterialDesignThemes.Wpf;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// A <see cref="ViewModel"/> containing all functionalities for the <see cref="Views.Scales.Main.ScaleWindow"/>
    /// </summary>
    public partial class ScaleWindowViewModel : ViewModel, IDialogHostViewModel
    {
        private readonly BusinessContext context;

        private Scale scale;

        /// <summary>
        /// Gets or sets a <see cref="Data.Scales.Scale"/> which is currently beeing edited
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

        private ScaleRange selectedRange;

        /// <summary>
        /// Gets or sets a selected <see cref="ScaleRange"/>
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

                if (value.Calibrations.Count == 0)
                {
                    TransitionerCalibrationSelectedIndex = 0;
                }
                else
                {
                    TransitionerCalibrationSelectedIndex = 1;
                }
            }
        }

        private Account account;

        /// <summary>
        /// Gets or sets an account which is currently manipulating over the <see cref="Scale"/>
        /// </summary>
        public Account Account
        {
            get
            {
                return account;
            }
            set
            {
                account = value;
                NotifyPropertyChanged(nameof(Account));
            }
        }

        private int transitionerSelectedIndex;

        /// <summary>
        /// Gets or sets an index of the <see cref="MaterialDesignThemes.Wpf.Transitions.Transitioner"/>
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

                //TODO close pop-ups
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ScaleWindowViewModel"/> class
        /// </summary>
        /// <param name="scale">A <see cref="Scale"/> which needs to be edited</param>
        /// <param name="context">A <see cref="BusinessContext"/> layer</param>
        public ScaleWindowViewModel(Scale scale, Account account, BusinessContext context)
        {
            this.context = context;

            Scale = scale;
            SelectedRange = Scale.Ranges.First();

            MessageQueue = new SnackbarMessageQueue();

            Calibrations = new ObservableCollection<ScaleCalibration>(SelectedRange.Calibrations);
            SelectedCalibration = SelectedRange.Calibrations.LastOrDefault();

            Account = account;
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

                #region Dialog closing actions

                if (!isDialogOpened && DialogViewModel != null)
                {
                    if (DialogViewModel.DialogResult.HasValue && DialogViewModel.DialogResult == true)
                    {
                        switch (DialogViewModel)
                        {
                            case NewCalibrationDialogViewModel calibrationDialogViewModel:
                                SelectedRange.Calibrations.Add(calibrationDialogViewModel.NewCalibration);
                                Calibrations.Add(calibrationDialogViewModel.NewCalibration);
                                SelectedCalibration = Calibrations.Last();

                                if (TransitionerCalibrationSelectedIndex == 0)
                                    TransitionerCalibrationSelectedIndex = 1;
                                break;

                            case NewRepeatabilityReferenceValueDialogViewModel repeatabilityReferenceValueDialogViewModel:
                                TransitionerRepeatabilitySelectedIndex = 1;
                                SelectedCalibration.Repeatability.ReferenceValue = repeatabilityReferenceValueDialogViewModel.ReferenceValue;

                                RepeatabilityTests = new ObservableCollection<ScaleRepeatabilityTest>(SelectedCalibration.Repeatability.ReferenceValue.Tests);
                                break;
                            //TODO dialog closing events

                            case NewRepeatabilityTestDialogViewModel repeatabilityTestDialogViewModel:
                                SelectedCalibration.Repeatability.ReferenceValue.Tests.Add(repeatabilityTestDialogViewModel.Test);
                                RepeatabilityTests.Add(repeatabilityTestDialogViewModel.Test);
                                
                                //TODO sredi chart
       
                                PrintRepeatabilityDataGridEndTest++;
                                break;

                            case NewAccuracyReferenceValueDialogViewModel scaleAccuracyReferenceValueDialogViewModel:
                                TransitionerAccuracySelectedIndex = 1;
                                SelectedCalibration.Accuracy.ReferenceValue = scaleAccuracyReferenceValueDialogViewModel.ReferenceValue;

                                //AccuracyTests = new ObservableCollection<Data.Scales.Accuracy.AccTest>(SelectedCalibration.Accuracy.ReferenceValue.Tests);
                                break;

                            case NewWeightDialogViewModel newScaleWeightDialogViewModel:
                                if (TransitionerSelectedIndex == 1)
                                {
                                    RepeatabilityWeights.Add(newScaleWeightDialogViewModel.NewScaleWeight);
                                    SelectedCalibration.Repeatability.ReferenceValue.Weights.Add(newScaleWeightDialogViewModel.NewScaleWeight);
                                }
                                else if (TransitionerSelectedIndex == 2)
                                {
                                    AccuracyWeights.Add(newScaleWeightDialogViewModel.NewScaleWeight);
                                    SelectedAccuracyReferenceValueMeasurement.Weights.Add(newScaleWeightDialogViewModel.NewScaleWeight);
                                }
                                else
                                {
                                    throw new System.Exception();
                                }
                                break;
                        }

                        context.UpdateScale(Scale);
                    }

                    DialogViewModel = null;
                    DialogContent = null;
                }

                #endregion
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
        /// Gets or sets a datacontext for the <see cref="DialogContent"/>
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

        #region Edit Scale Members

        /// <summary>
        /// Gets an <see cref="ICommand"/> for opening an <see cref="Views.Scales.EditScaleDialog"/>
        /// </summary>
        public ICommand OpenEditScaleDialogCommand
        {
            get
            {
                return new ActionCommand(a => OpenEditScaleDialog(), p => Account is Administrator);
            }
        }

        /// <summary>
        /// Opnes an <see cref="Views.Scales.EditScaleDialog"/>
        /// </summary>
        private void OpenEditScaleDialog()
        {
            DialogViewModel = new EditScaleDialogViewModel(Scale, this);

            DialogContent = new Views.Scales.Dialogs.EditScaleDialog()
            {
                DataContext = DialogViewModel
            };

            IsDialogOpened = true;
        }

        #endregion

        #region User Accesses Members

        /// <summary>
        /// Gets an <see cref="ICommand"/> for opening a <see cref="Views.Scales.Dialogs.UserAccessesDialog"/>
        /// </summary>
        public ICommand OpenUserAccessesDialogCommand
        {
            get
            {
                return new ActionCommand(a => OpenUserAccessesDialog(), p => Account is Administrator);
            }
        }

        /// <summary>
        /// Opens a <see cref="Views.Scales.Dialogs.UserAccessesDialog"/>
        /// </summary>
        private void OpenUserAccessesDialog()
        {
            DialogViewModel = new UserAccessesDialogViewModel(context.GetUsers(), Scale, this, context);

            DialogContent = new Views.Scales.Dialogs.UserAccessesDialog()
            {
                DataContext = DialogViewModel
            };

            IsDialogOpened = true;
        }

        #endregion
    }
}
