using InstrumentManagement.Windows;
using LiveCharts;
using LiveCharts.Configurations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InstrumentManagement.DesktopClient.ViewModels.Scales.Main
{
    public partial class ScaleWindowViewModel
    {
        private ChartValues<double> repeatabilityChartValues;

        /// <summary>
        /// Gets or sets a list of double values for the chart series
        /// </summary>
        public ChartValues<double> RepeatabilityChartValues
        {
            get
            {
                return repeatabilityChartValues;
            }
            set
            {
                repeatabilityChartValues = value;
                NotifyPropertyChanged(nameof(RepeatabilityChartValues));
            }
        }

        private ICollection<string> repeatabilityChartLabels;

        /// <summary>
        /// Gets or sets a list of labels for the chart
        /// </summary>
        public ICollection<string> RepeatabilityChartLabels
        {
            get
            {
                return repeatabilityChartLabels;
            }
            set
            {
                repeatabilityChartLabels = value;
                NotifyPropertyChanged(nameof(RepeatabilityChartLabels));
            }
        }

        private CartesianMapper<double> repeatabilityChartMapper;

        /// <summary>
        /// Gets or sets a mapper for the chart
        /// </summary>
        public CartesianMapper<double> RepeatabilityChartMapper
        {
            get
            {
                return repeatabilityChartMapper;
            }
            set
            {
                repeatabilityChartMapper = value;
                NotifyPropertyChanged(nameof(RepeatabilityChartMapper));
            }
        }

        #region Min/Max Values

        private double repeatabilityChartAxisXMaxValue;

        /// <summary>
        /// Gets or sets a max value for chart axis x
        /// </summary>
        public double RepeatabilityChartAxisXMaxValue
        {
            get
            {
                return repeatabilityChartAxisXMaxValue;
            }
            set
            {
                if (value > RepeatabilityTests.Count)
                {
                    repeatabilityChartAxisXMaxValue = RepeatabilityTests.Count;
                }
                else if (value < 1)
                {
                    repeatabilityChartAxisXMaxValue = 1;
                }
                else
                {
                    repeatabilityChartAxisXMaxValue = value;
                }

                NotifyPropertyChanged(nameof(RepeatabilityChartAxisXMaxValue));

                RepeatabilityChartSetAxisYMaxValue();
            }
        }

        private double repeatabilityChartAxisXMinValue;

        /// <summary>
        /// Gets or sets a min value for chart axis x
        /// </summary>
        public double RepeatabilityChartAxisXMinValue
        {
            get
            {
                return repeatabilityChartAxisXMinValue;
            }
            set
            {
                if (value < 0)
                {
                    repeatabilityChartAxisXMinValue = 0;
                }
                else if (value > RepeatabilityTests.Count - 1)
                {
                    repeatabilityChartAxisXMinValue = RepeatabilityTests.Count - 1;
                }
                else
                {
                    repeatabilityChartAxisXMinValue = value;
                }

                NotifyPropertyChanged(nameof(RepeatabilityChartAxisXMinValue));

                RepeatabilityChartSetAxisYMaxValue();
            }
        }

        private double repeatabilityChartAxisYMaxValue;

        /// <summary>
        /// Gets or sets a max value for chart axis y
        /// </summary>
        public double RepeatabilityChartAxisYMaxValue
        {
            get
            {
                return repeatabilityChartAxisYMaxValue;
            }
            set
            {
                repeatabilityChartAxisYMaxValue = value;
                NotifyPropertyChanged(nameof(RepeatabilityChartAxisYMaxValue));
            }
        }

        #endregion

        /// <summary>
        /// Sets axis y max value for selected interval
        /// </summary>
        private void RepeatabilityChartSetAxisYMaxValue()
        {
            double maxValue = RepeatabilityChartValues.Skip(Convert.ToInt32(Math.Ceiling(RepeatabilityChartAxisXMinValue))).Take(Convert.ToInt32(Math.Floor(RepeatabilityChartAxisXMaxValue))).Max();
            double maxValidValue = SelectedCalibration.Repeatability.ReferenceValue.MaxValidValue;

            RepeatabilityChartAxisYMaxValue = 1.05 * (maxValue > maxValidValue ? maxValue : maxValidValue);
        }

        private bool repeatabilityChartDataLabelVisible;

        /// <summary>
        /// Gets or sets a visiblity of repeatability chart data labels
        /// </summary>
        public bool RepeatabilityChartDataLabelVisible
        {
            get
            {
                return repeatabilityChartDataLabelVisible;
            }
            set
            {
                repeatabilityChartDataLabelVisible = value;
                NotifyPropertyChanged(nameof(RepeatabilityChartDataLabelVisible));
            }
        }

        /// <summary>
        /// Gets an <see cref="ICommand"/> for round axis x values
        /// </summary>
        public ICommand RepeatabilityChartSetAxisXRoundCommand
        {
            get
            {
                return new ActionCommand(a => RepeatabilityChartSetAxisXRound());
            }
        }

        /// <summary>
        /// Rounds axis x values
        /// </summary>
        private void RepeatabilityChartSetAxisXRound()
        {
            RepeatabilityChartAxisXMaxValue = Math.Round(RepeatabilityChartAxisXMaxValue);
            RepeatabilityChartAxisXMinValue = Math.Round(RepeatabilityChartAxisXMinValue);
        }
    }
}
