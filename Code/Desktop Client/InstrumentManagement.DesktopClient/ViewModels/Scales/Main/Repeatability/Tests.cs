namespace InstrumentManagement.DesktopClient.ViewModels.Scales.Main
{
    using InstrumentManagement.Data.Scales.Repeatability;
    using InstrumentManagement.Windows;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Controls;
    using System.Windows.Input;

    public partial class ScaleWindowViewModel
    {
        private ICollection<ScaleRepeatabilityTest> repeatabilityTests;

        /// <summary>
        /// Gets or sets a list of <see cref="ScaleRepeatabilityTest"/>
        /// </summary>
        public ICollection<ScaleRepeatabilityTest> RepeatabilityTests
        {
            get
            {
                return repeatabilityTests;
            }
            set
            {
                repeatabilityTests = value;
                NotifyPropertyChanged(nameof(RepeatabilityTests));

                SelectedRepeatabilityTest = value.LastOrDefault();
            }
        }

        private ScaleRepeatabilityTest selectedRepeatabilityTest;

        /// <summary>
        /// Gets or sets a selected <see cref="ScaleRepeatabilityTest"/> from the <see cref="RepeatabilityTests"/>
        /// </summary>
        public ScaleRepeatabilityTest SelectedRepeatabilityTest
        {
            get
            {
                return selectedRepeatabilityTest;
            }
            set
            {
                selectedRepeatabilityTest = value;
                NotifyPropertyChanged(nameof(SelectedRepeatabilityTest));
            }
        }

        private bool isRepeatabilityPopupDataGridPrintingOpen;

        /// <summary>
        /// Gets or sets if popup for data grid repeatability printing is opened
        /// </summary>
        public bool IsPopupRepeatabilityDataGridPrintingOpen
        {
            get
            {
                return isRepeatabilityPopupDataGridPrintingOpen;
            }
            set
            {
                isRepeatabilityPopupDataGridPrintingOpen = value;
                NotifyPropertyChanged(nameof(IsPopupRepeatabilityDataGridPrintingOpen));
            }
        }

        /// <summary>
        /// Gets an <see cref="ICommand"/> for opening a popup data grid print
        /// </summary>
        public ICommand OpenPopupRepeatabilityDataGridPrinting
        {
            get
            {
                return new ActionCommand(a => IsPopupRepeatabilityDataGridPrintingOpen = true);
            }
        }

        private int printRepeatabilityDataGridStartTest;

        /// <summary>
        /// Gets or sets a start test for printing data grid
        /// </summary>
        public int PrintRepeatabilityDataGridStartTest
        {
            get
            {
                return printRepeatabilityDataGridStartTest;
            }
            set
            {
                printRepeatabilityDataGridStartTest = value;
                NotifyPropertyChanged(nameof(PrintRepeatabilityDataGridStartTest));
            }
        }

        private int printRepeatabilityDataGridEndTest;

        /// <summary>
        /// Gets or sets a end test for printing data grid
        /// </summary>
        public int PrintRepeatabilityDataGridEndTest
        {
            get
            {
                return printRepeatabilityDataGridEndTest;
            }
            set
            {
                printRepeatabilityDataGridEndTest = value;
                NotifyPropertyChanged(nameof(PrintRepeatabilityDataGridEndTest));
            }
        }

        /// <summary>
        /// Prints a repeatability data grid
        /// </summary>
        public void PrintRepeatabilityDataGrid(DataGrid dataGrid)
        {
            
        }
    }
}
