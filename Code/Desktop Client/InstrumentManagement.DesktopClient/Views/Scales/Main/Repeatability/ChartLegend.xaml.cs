namespace InstrumentManagement.DesktopClient.Views.Scales.Main.Repeatability
{
    using LiveCharts.Wpf;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for ChartLegend.xaml
    /// </summary>
    public partial class ChartLegend : UserControl, IChartLegend
    {
        public ChartLegend()
        {
            InitializeComponent();
        }

        private List<SeriesViewModel> series;

        public List<SeriesViewModel> Series
        {
            get
            {
                return series;
            }
            set
            {
                series = value;
                NotifyPropertyChanged(nameof(Series));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
