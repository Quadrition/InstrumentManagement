namespace InstrumentManagement.DesktopClient.ViewModels.Scales.Main
{
    using InstrumentManagement.Windows;
    using LiveCharts;
    using LiveCharts.Configurations;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

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

        #region Set Range Members

        private DateTime repeatabilityChartStartDate;

        /// <summary>
        /// Gets or sets a start date for setting chart range
        /// </summary>
        public DateTime RepeatabilityChartStartDate
        {
            get
            {
                return repeatabilityChartStartDate;
            }
            set
            {
                repeatabilityChartStartDate = value;
                NotifyPropertyChanged(nameof(RepeatabilityChartStartDate));
            }
        }

        private DateTime repeatabilityChartEndDate;

        /// <summary>
        /// Gets or sets an end date for setting chart range
        /// </summary>
        public DateTime RepeatabilityChartEndDate
        {
            get
            {
                return repeatabilityChartEndDate;
            }
            set
            {
                repeatabilityChartEndDate = value;
                NotifyPropertyChanged(nameof(RepeatabilityChartEndDate));
            }
        }

        /// <summary>
        /// Gets an <see cref="ICommand"/> for setting date range on repeatability chart
        /// </summary>
        public ICommand SetDateRangeRepeatabilityChartCommand
        {
            get
            {
                return new ActionCommand(a => SetDateRangeRepeatabilityChart(), p => RepeatabilityTests != null && RepeatabilityTests.Count != 0);
            }
        }

        /// <summary>
        /// Sets a date range on repeatability chart
        /// </summary>
        private void SetDateRangeRepeatabilityChart()
        {
            RepeatabilityChartStartDate = new DateTime(RepeatabilityChartStartDate.Year, RepeatabilityChartStartDate.Month, 1);
            RepeatabilityChartEndDate = new DateTime(RepeatabilityChartEndDate.Year, RepeatabilityChartEndDate.Month, DateTime.DaysInMonth(RepeatabilityChartEndDate.Year, RepeatabilityChartEndDate.Month));

            if (RepeatabilityChartStartDate < RepeatabilityChartEndDate)
            {
                if (RepeatabilityChartStartDate > RepeatabilityTests.Last().Date)
                {
                    MessageQueue.Enqueue("Početni datum je veći od datuma poslednjeg testa");
                }
                else if (RepeatabilityChartEndDate < RepeatabilityTests.First().Date)
                {
                    MessageQueue.Enqueue("Krajnji datum je manji od datuma prvog testa");
                }
                else
                {
                    if (RepeatabilityChartStartDate < RepeatabilityTests.First().Date)
                    {
                        RepeatabilityChartAxisXMinValue = 0;
                    }
                    else
                    {
                        for (int i = 0; i < RepeatabilityChartLabels.Count; i++)
                        {
                            if (RepeatabilityChartLabels.ElementAt(i) == RepeatabilityChartStartDate.ToString("MMM yy"))
                            {
                                RepeatabilityChartAxisXMinValue = i;
                                break;
                            }

                            if (i == RepeatabilityChartLabels.Count - 1)
                            {
                                RepeatabilityChartAxisXMinValue = 0;
                            }
                        }
                    }

                    if (RepeatabilityChartEndDate > RepeatabilityTests.Last().Date)
                    {
                        RepeatabilityChartAxisXMaxValue = RepeatabilityTests.Count;
                    }
                    else
                    {
                        for (int i = RepeatabilityChartLabels.Count - 1; i >= 0; i--)
                        {
                            if (RepeatabilityChartLabels.ElementAt(i) == RepeatabilityChartEndDate.ToString("MMM yy"))
                            {
                                RepeatabilityChartAxisXMaxValue = RepeatabilityChartAxisXMinValue == i ? i + 1 : i;
                                break;
                            }

                            if (i == 0)
                            {
                                RepeatabilityChartAxisXMaxValue = RepeatabilityTests.Count;
                            }
                        }
                    }
                }
            }
            else
            {
                MessageQueue.Enqueue("Početni datum mora biti veći od krajnjeg");
            }
        }

        #endregion

        /// <summary>
        /// Sets axis repeatability chart y max value for selected interval
        /// </summary>
        private void RepeatabilityChartSetAxisYMaxValue()
        {
            double maxValidValue = SelectedCalibration.Repeatability.ReferenceValue.MaxValidValue;

            if (RepeatabilityChartValues.Count != 0)
            {
                double maxValue = RepeatabilityChartValues.Skip(Convert.ToInt32(Math.Ceiling(RepeatabilityChartAxisXMinValue))).Take(Convert.ToInt32(Math.Floor(RepeatabilityChartAxisXMaxValue))).Max();

                RepeatabilityChartAxisYMaxValue = 1.05 * (maxValue > maxValidValue ? maxValue : maxValidValue);
            }
            else
            {
                RepeatabilityChartAxisYMaxValue = maxValidValue;
            }
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
        //TODO add printing
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
