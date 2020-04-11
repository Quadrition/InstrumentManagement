using InstrumentManagement.Windows;
using LiveCharts;
using LiveCharts.Configurations;
using System;
using System.Collections.Generic;
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
    }
}
