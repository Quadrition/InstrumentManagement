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
    }
}
