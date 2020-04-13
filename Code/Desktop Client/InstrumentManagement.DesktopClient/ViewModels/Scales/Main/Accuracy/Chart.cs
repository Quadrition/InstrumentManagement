namespace InstrumentManagement.DesktopClient.ViewModels.Scales.Main
{
    using InstrumentManagement.Data.Scales.Accuracy;
    using InstrumentManagement.DesktopClient.Views.Scales.Main.Accuracy;
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

        #region Set Range Members

        private short accuracyChartStartNumber;

        public short AccuracyChartStartNumber
        {
            get
            {
                return accuracyChartStartNumber;
            }
            set
            {
                accuracyChartStartNumber = value;
                NotifyPropertyChanged(nameof(AccuracyChartStartNumber));
            }
        }

        private short accuracyChartEndNumber;

        public short AccuracyChartEndNumber
        {
            get
            {
                return accuracyChartEndNumber;
            }
            set
            {
                accuracyChartEndNumber = value;
                NotifyPropertyChanged(nameof(AccuracyChartEndNumber));
            }
        }

        /// <summary>
        /// Gets an <see cref="ICommand"/> for setting date range on accuracy chart
        /// </summary>
        public ICommand SetNumberRangeAccuracyChartCommand
        {
            get
            {
                return new ActionCommand(a => SetNumberRangeAccuracyChart(), p => AccuracyTests != null && AccuracyTests.Count != 0);
            }
        }

        /// <summary>
        /// Sets a date range on repeatability chart
        /// </summary>
        private void SetNumberRangeAccuracyChart()
        {
            if (AccuracyChartStartNumber < AccuracyChartEndNumber)
            {
                if (AccuracyChartStartNumber > AccuracyTests.Last().Number)
                {
                    MessageQueue.Enqueue("Početni broj testa je veći od broja poslednjeg testa");
                }
                else if (AccuracyChartEndNumber < AccuracyTests.First().Number)
                {
                    MessageQueue.Enqueue("Krajnji broj testa je manji od broja prvog testa");
                }
                else
                {
                    if (AccuracyChartStartNumber < AccuracyTests.First().Number)
                    {
                        AccuracyChartAxisXMinValue = 0;
                    }
                    else
                    {
                        for (int i = 0; i < AccuracyChartLabels.Count; i++)
                        {
                            if (Convert.ToInt32(AccuracyChartLabels.ElementAt(i)) == AccuracyChartStartNumber)
                            {
                                AccuracyChartAxisXMinValue = i;
                                break;
                            }

                            if (i == AccuracyChartLabels.Count - 1)
                            {
                                AccuracyChartAxisXMinValue = 0;
                            }
                        }
                    }

                    if (AccuracyChartEndNumber > AccuracyTests.Last().Number)
                    {
                        AccuracyChartAxisXMaxValue = AccuracyTests.Count;
                    }
                    else
                    {
                        for (int i = AccuracyChartLabels.Count - 1; i >= 0; i--)
                        {
                            if (Convert.ToInt32(AccuracyChartLabels.ElementAt(i)) == AccuracyChartEndNumber)
                            {
                                AccuracyChartAxisXMaxValue = AccuracyChartAxisXMinValue == i ? i + 1 : i;
                                break;
                            }

                            if (i == AccuracyChartLabels.Count - 1)
                            {
                                AccuracyChartAxisXMaxValue = AccuracyTests.Count;
                            }
                        }
                    }
                }
            }
            else
            {
                MessageQueue.Enqueue("Početni broj testa mora biti veći od krajnjeg");
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

        /// <summary>
        /// Rounds axis x values
        /// </summary>
        private void AccuracyChartSetAxisXRound()
        {
            AccuracyChartAxisXMaxValue = Math.Round(AccuracyChartAxisXMaxValue);
            AccuracyChartAxisXMinValue = Math.Round(AccuracyChartAxisXMinValue);
        }

        /// <summary>
        /// Gets an <see cref="ICommand"/> for printing accuracy chart
        /// </summary>
        public ICommand PrintAccuracyChartCommand
        {
            get
            {
                return new ActionCommand(a => PrintAccuracyChart());
            }
        }

        /// <summary>
        /// Create accuracy chart for printing
        /// </summary>
        private CartesianChart CreateAccuracyChart()
        {
            var chartLegend = new ChartLegend();

            chartLegend.ScaleManufacturerTextBlock.Text = Scale.Manufacturer;
            chartLegend.ScaleTypeTextBlock.Text = string.IsNullOrEmpty(Scale.Type) ? null : "/" + Scale.Type;
            chartLegend.ScaleSerialNumberTextBlock.Text = string.IsNullOrEmpty(Scale.SerialNumber) ? null : "/" + Scale.SerialNumber;
            chartLegend.RangeUpperValueTextBlock.Text = Convert.ToString(SelectedRange.UpperValue);
            chartLegend.RangeLowerValueTextBlock.Text = string.IsNullOrEmpty(Convert.ToString(SelectedRange.LowerValue)) ? null : "/" + Scale.Type;
            chartLegend.RangeGraduateTextBlock.Text = string.IsNullOrEmpty(Convert.ToString(SelectedRange.Graduate)) ? null : "/" + Scale.Type;
            chartLegend.CalibrationNumberTextBlock.Text = Convert.ToString(SelectedCalibration.Number);
            chartLegend.VerificationNumberTextBlock.Text = SelectedCalibration.Verification.NumberOfVerification;
            chartLegend.WeightsItemsControl.ItemsSource = AccuracyWeights;
            chartLegend.MaxValueTextBlock.Text = Convert.ToString(AccuracyChartMeasurement.MaxValidValue);
            chartLegend.MinValueTextBlock.Text = Convert.ToString(AccuracyChartMeasurement.MinValidValue);

            var chart = new CartesianChart()
            {
                DisableAnimations = true,
                LegendLocation = LegendLocation.Right,
                ChartLegend = chartLegend,
                Width = 1050,
                Height = 550,
                Background = new SolidColorBrush(Colors.White),
                Series = new SeriesCollection
                {
                    new LineSeries
                    {
                        Values = new ChartValues<double>(AccuracyChartValues),
                        Fill = new SolidColorBrush(Colors.Transparent),
                        DataLabels = AccuracyChartDataLabelVisible,
                        PointGeometrySize = 10,
                        PointGeometry = DefaultGeometries.Square,
                        Configuration = AccuracyChartMapper
                    }
                },
                AxisX = new AxesCollection()
                {
                    new Axis()
                    {
                        Title = "Redni broj testa",
                        Labels = new List<string>(AccuracyChartLabels),
                        MinValue = AccuracyChartAxisXMinValue,
                        MaxValue = AccuracyChartAxisXMaxValue
                    }
                },
                AxisY = new AxesCollection()
                {
                    new Axis()
                    {
                        MinValue = AccuracyChartAxisYMinValue,
                        MaxValue = AccuracyChartAxisYMaxValue,
                        Title = "Standardna devijacija",
                        Sections = new SectionsCollection()
                        {
                            new AxisSection()
                            {
                                Value = AccuracyChartMeasurement.MinValidValue,
                                StrokeThickness = 1,
                                DataLabel = false,
                                Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF8585"))
                            },
                            new AxisSection()
                            {
                                Value = AccuracyChartMeasurement.MaxValidValue,
                                StrokeThickness = 1,
                                DataLabel = false,
                                Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF8585"))
                            }
                        },
                        Separator = new LiveCharts.Wpf.Separator()
                        {
                            Step = SelectedRange.Graduate
                        }
                    }
                }
            };

            var viewbox = new Viewbox
            {
                Child = chart
            };
            viewbox.Measure(chart.RenderSize);
            viewbox.Arrange(new Rect(new Point(0, 0), chart.RenderSize));
            chart.Update(true, true); //force chart redraw
            viewbox.UpdateLayout();
            viewbox.Child = null;

            return chart;
        }

        /// <summary>
        /// Prints accuracy chart
        /// </summary>
        private void PrintAccuracyChart()
        {
            CartesianChart chart = CreateAccuracyChart();

            PrintDialog dialog = new PrintDialog();

            if (dialog.ShowDialog() == true)
            {
                dialog.PrintVisual(chart, "Grafički prikaz testa ponovljivosti vage");
            }
        }
    }
}
