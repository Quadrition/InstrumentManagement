using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InstrumentManagement.DesktopClient.Views.Scales.Main.Repeatability
{
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
