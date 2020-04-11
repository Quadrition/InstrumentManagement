namespace InstrumentManagement.DesktopClient.ViewModels.Scales.Main
{
    using InstrumentManagement.Data.Accounts;
    using InstrumentManagement.Data.Scales;
    using InstrumentManagement.Data.Scales.Accuracy;
    using InstrumentManagement.Data.Scales.Calibration;
    using InstrumentManagement.Data.Scales.Repeatability;
    using InstrumentManagement.DesktopClient.ViewModels.Scales.Dialogs;
    using InstrumentManagement.Windows;
    using LiveCharts;
    using LiveCharts.Configurations;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using System.Windows.Media;

    public partial class ScaleWindowViewModel
    {
        /// <summary>
        /// Gets or sets a list of <see cref="ScaleCalibration"/>
        /// </summary>
        public ICollection<ScaleCalibration> Calibrations { get; set; }

        private ScaleCalibration selectedCalibration;

        /// <summary>
        /// Gets or sets a selected <see cref="ScaleCalibration"/>
        /// </summary>
        public ScaleCalibration SelectedCalibration
        {
            get
            {
                return selectedCalibration;
            }
            set
            {
                selectedCalibration = value;
                NotifyPropertyChanged(nameof(SelectedCalibration));

                #region Change Calibration actions

                TransitionerSelectedIndex = 0;
                //TODO proveri kad menjas opsege pa sta se desi

                if (value != null)
                {
                    if (SelectedCalibration.Repeatability.ReferenceValue == null)
                    {
                        TransitionerRepeatabilitySelectedIndex = 0;
                    }
                    else
                    {
                        TransitionerRepeatabilitySelectedIndex = 2;

                        RepeatabilityWeights = new ObservableCollection<ScaleWeight>(SelectedCalibration.Repeatability.ReferenceValue.Weights);

                        RepeatabilityTests = new ObservableCollection<ScaleRepeatabilityTest>(SelectedCalibration.Repeatability.ReferenceValue.Tests);

                        RepeatabilityChartValues = new ChartValues<double>(SelectedCalibration.Repeatability.ReferenceValue.Tests.Select(test => test.StandardDeviation));
                        RepeatabilityChartLabels = new ObservableCollection<string>(SelectedCalibration.Repeatability.ReferenceValue.Tests.Select(test => test.Date.ToString("MMM yy")));
                        RepeatabilityChartMapper = Mappers.Xy<double>().X((item, index) => index).Y(item => item).Fill(item => item > SelectedCalibration.Repeatability.ReferenceValue.MaxValidValue ? new SolidColorBrush(Color.FromRgb(238, 83, 80)) : null).Stroke(item => item > SelectedCalibration.Repeatability.ReferenceValue.MaxValidValue ? new SolidColorBrush(Color.FromRgb(238, 83, 80)) : null);

                        PrintRepeatabilityDataGridStartTest = 1;
                        PrintRepeatabilityDataGridEndTest = RepeatabilityTests.Count;
                    }

                    if (SelectedCalibration.Accuracy.ReferenceValue == null)
                    {
                        TransitionerAccuracySelectedIndex = 0;
                    }
                    else
                    {
                        TransitionerAccuracySelectedIndex = 2;

                        AccuracyTests = new ObservableCollection<ScaleAccuracyTest>(SelectedCalibration.Accuracy.ReferenceValue.Tests);

                        SelectedAccuracyReferenceValueMeasurement = SelectedCalibration.Accuracy.ReferenceValue.Measurements.First();
                        SelectedAccuracyTestMeasurement = SelectedCalibration.Accuracy.ReferenceValue.Measurements.First();

                        PrintAccuracyDataGridStartTest = 1;
                        PrintAccuracyDataGridEndTest = AccuracyTests.Count;
                    }
                }

                #endregion
            }
        }

        /// <summary>
        /// Checks if the last <see cref="ScaleCalibration"/> is selected
        /// </summary>
        public bool IsLastCalibration
        {
            get
            {
                if (SelectedRange.Calibrations.Count == 0)
                {
                    return false;
                }

                return SelectedCalibration == SelectedRange.Calibrations.Last();
            }
        }

        /// <summary>
        /// Gets an <see cref="ICommand"/> for showing a <see cref="ScaleCalibration"/>
        /// </summary>
        public ICommand ShowCalibrationCommand
        {
            get
            {
                return new ActionCommand(a => TransitionerSelectedIndex = 0);
            }
        }

        private int transitionerCalibrationSelectedIndex;

        /// <summary>
        /// Gets or sets a selected index of <see cref="MaterialDesignThemes.Wpf.Transitions.Transitioner"/> in calibration
        /// </summary>
        public int TransitionerCalibrationSelectedIndex
        {
            get
            {
                return transitionerCalibrationSelectedIndex;
            }
            set
            {
                transitionerCalibrationSelectedIndex = value;
                NotifyPropertyChanged(nameof(TransitionerCalibrationSelectedIndex));
            }
        }

        #region New Calibration Members

        /// <summary>
        /// Gets an <see cref="ICommand"/> for opening a <see cref="Views.NewScaleCalibrationDialog"/>
        /// </summary>
        public ICommand OpenNewScaleCalibrationDialogCommand
        {
            get
            {
                return new ActionCommand(a => OpenNewScaleCalibrationDialog(), p => Account is Administrator);
            }
        }

        /// <summary>
        /// Opnes a <see cref="Views.Scales.Dialogs.NewScaleCalibrationDialog"/>
        /// </summary>
        private void OpenNewScaleCalibrationDialog()
        {
            DialogViewModel = new NewCalibrationDialogViewModel(SelectedRange, this);

            DialogContent = new Views.Scales.Dialogs.NewCalibrationDialog()
            {
                DataContext = DialogViewModel
            };

            IsDialogOpened = true;
        }

        #endregion
    }
}
