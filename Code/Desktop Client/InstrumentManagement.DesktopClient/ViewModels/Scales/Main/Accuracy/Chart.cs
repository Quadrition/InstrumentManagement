namespace InstrumentManagement.DesktopClient.ViewModels.Scales.Main
{
    using InstrumentManagement.Data.Scales.Accuracy;
    using InstrumentManagement.Windows;
    using LiveCharts;
    using LiveCharts.Configurations;
    using LiveCharts.Wpf;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    public partial class ScaleWindowViewModel
    {
        private ScaleAccuracyReferenceValueMeasurement accuracyChartMeasurement;

        /// <summary>
        /// Gets or sets a <see cref="ScaleAccuracyReferenceValueMeasurement"/> for the accuracy chart
        /// </summary>
        public ScaleAccuracyReferenceValueMeasurement AccuracyChartMeasurement
        {
            get
            {
                return accuracyChartMeasurement;
            }
            set
            {
                accuracyChartMeasurement = value;
                NotifyPropertyChanged(nameof(AccuracyChartMeasurement));

                if (value != null)
                {
                    AccuracyChartMapper = Mappers.Xy<double>().X((item, index) => index).Y(item => item).Fill(item => (item > value.MaxValidValue || item < value.MinValidValue) ? new SolidColorBrush(Color.FromRgb(238, 83, 80)) : null).Stroke(item => (item > value.MaxValidValue || item < value.MinValidValue) ? new SolidColorBrush(Color.FromRgb(238, 83, 80)) : null);
                    AccuracyChartValues = new ChartValues<double>(value.TestMeasurements.Select(test => test.Result));

                    SetAccuracyChartSetAxisYValues();
                }
            }
        }

        private ChartValues<double> accuracyChartValues;

        /// <summary>
        /// Gets or sets a list of double values for the accuracy chart series
        /// </summary>
        public ChartValues<double> AccuracyChartValues
        {
            get
            {
                return accuracyChartValues;
            }
            set
            {
                accuracyChartValues = value;
                NotifyPropertyChanged(nameof(AccuracyChartValues));
            }
        }

        private ICollection<string> accuracyChartLabels;

        /// <summary>
        /// Gets or sets a list of labels for the accuracy chart
        /// </summary>
        public ICollection<string> AccuracyChartLabels
        {
            get
            {
                return accuracyChartLabels;
            }
            set
            {
                accuracyChartLabels = value;
                NotifyPropertyChanged(nameof(AccuracyChartLabels));
            }
        }

        private CartesianMapper<double> accuracyChartMapper;

        /// <summary>
        /// Gets or sets a mapper for the accuracy chart
        /// </summary>
        public CartesianMapper<double> AccuracyChartMapper
        {
            get
            {
                return accuracyChartMapper;
            }
            set
            {
                accuracyChartMapper = value;
                NotifyPropertyChanged(nameof(AccuracyChartMapper));
            }
        }

        #region Min/Max Values

        private double accuracyChartAxisXMaxValue;

        /// <summary>
        /// Gets or sets a max value for chart axis x
        /// </summary>
        public double AccuracyChartAxisXMaxValue
        {
            get
            {
                return accuracyChartAxisXMaxValue;
            }
            set
            {
                if (value > AccuracyTests.Count)
                {
                    accuracyChartAxisXMaxValue = AccuracyTests.Count;
                }
                else if (value < 1)
                {
                    accuracyChartAxisXMaxValue = 1;
                }
                else
                {
                    accuracyChartAxisXMaxValue = value;
                }

                NotifyPropertyChanged(nameof(AccuracyChartAxisXMaxValue));

                SetAccuracyChartSetAxisYValues();
            }
        }

        private double accuracyChartAxisXMinValue;

        /// <summary>
        /// Gets or sets a min value for chart axis x
        /// </summary>
        public double AccuracyChartAxisXMinValue
        {
            get
            {
                return accuracyChartAxisXMinValue;
            }
            set
            {
                if (value < 0)
                {
                    accuracyChartAxisXMinValue = 0;
                }
                else if (value > AccuracyTests.Count - 1)
                {
                    accuracyChartAxisXMinValue = AccuracyTests.Count - 1;
                }
                else
                {
                    accuracyChartAxisXMinValue = value;
                }

                NotifyPropertyChanged(nameof(AccuracyChartAxisXMinValue));

                SetAccuracyChartSetAxisYValues();
            }
        }

        private double accuracyChartAxisYMaxValue;

        /// <summary>
        /// Gets or sets a max value for chart axis y
        /// </summary>
        public double AccuracyChartAxisYMaxValue
        {
            get
            {
                return accuracyChartAxisYMaxValue;
            }
            set
            {
                accuracyChartAxisYMaxValue = value;
                NotifyPropertyChanged(nameof(AccuracyChartAxisYMaxValue));
            }
        }

        private double accuracyChartAxisYMinValue;

        /// <summary>
        /// Gets or sets a min value for chart axis y
        /// </summary>
        public double AccuracyChartAxisYMinValue
        {
            get
            {
                return accuracyChartAxisYMinValue;
            }
            set
            {
                accuracyChartAxisYMinValue = value;
                NotifyPropertyChanged(nameof(AccuracyChartAxisYMinValue));
            }
        }

        #endregion

        /// <summary>
        /// Gets a width for valid section in accuracy chart
        /// </summary>
        public double AccuracyChartSectionWidth
        {
            get
            {
                return AccuracyChartMeasurement.MaxValidValue - AccuracyChartMeasurement.MinValidValue;
            }
        }

        /// <summary>
        /// Sets axis accuracy chart y max value for selected interval
        /// </summary>
        private void SetAccuracyChartSetAxisYValues()
        {
            double maxValidValue = AccuracyChartMeasurement.MaxValidValue;
            double minValidValue = AccuracyChartMeasurement.MinValidValue;

            if (AccuracyChartValues.Count != 0 && AccuracyChartAxisXMaxValue != AccuracyChartAxisXMinValue)
            {
                IEnumerable<double> values = AccuracyChartValues.Skip(Convert.ToInt32(Math.Ceiling(AccuracyChartAxisXMinValue))).Take(Convert.ToInt32(Math.Floor(AccuracyChartAxisXMaxValue)));

                double maxValue = values.Max();
                double minValue = values.Min();

                AccuracyChartAxisYMaxValue = (maxValue > maxValidValue ? maxValue : maxValidValue) + SelectedRange.Graduate;
                AccuracyChartAxisYMinValue = (minValue < minValidValue ? minValue : minValidValue) - SelectedRange.Graduate;
            }
            else
            {
                AccuracyChartAxisYMaxValue = maxValidValue + SelectedRange.Graduate;
                AccuracyChartAxisYMinValue = minValidValue - SelectedRange.Graduate;
            }
        }

        private bool accuracyChartDataLabelVisible;

        /// <summary>
        /// Gets or sets a visiblity of accuyracy chart data labels
        /// </summary>
        public bool AccuracyChartDataLabelVisible
        {
            get
            {
                return accuracyChartDataLabelVisible;
            }
            set
            {
                accuracyChartDataLabelVisible = value;
                NotifyPropertyChanged(nameof(AccuracyChartDataLabelVisible));
            }
        }

        /// <summary>
        /// Gets an <see cref="ICommand"/> for round axis x values
        /// </summary>
        public ICommand AccuracyChartSetAxisXRoundCommand
        {
            get
            {
                return new ActionCommand(a => AccuracyChartSetAxisXRound());
            }
        }
        //TODO add printing
        /// <summary>
        /// Rounds axis x values
        /// </summary>
        private void AccuracyChartSetAxisXRound()
        {
            AccuracyChartAxisXMaxValue = Math.Round(AccuracyChartAxisXMaxValue);
            AccuracyChartAxisXMinValue = Math.Round(AccuracyChartAxisXMinValue);
        }
    }
}
