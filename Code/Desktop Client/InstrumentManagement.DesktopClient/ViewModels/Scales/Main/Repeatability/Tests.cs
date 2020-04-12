namespace InstrumentManagement.DesktopClient.ViewModels.Scales.Main
{
    using InstrumentManagement.Data.Scales.Repeatability;
    using InstrumentManagement.DesktopClient.ViewModels.Scales.Dialogs;
    using InstrumentManagement.Windows;
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

        /// <summary>
        /// Gets an <see cref="ICommand"/> for opening a <see cref="Views.Scales.Dialogs.NewRepeatabilityTestDialog"/>
        /// </summary>
        public ICommand ShowNewScaleRepeatabilityTestDialogCommand
        {
            get
            {
                return new ActionCommand(a => ShowNewScaleRepeatabilityTestDialog(), p => IsLastCalibration == true && SelectedCalibration.Repeatability.ReferenceValue != null);
            }
        }

        /// <summary>
        /// Opens a <see cref="Views.Scales.Dialogs.NewRepeatabilityTestDialog"/>
        /// </summary>
        private void ShowNewScaleRepeatabilityTestDialog()
        {
            //for (int i = 0; i < 20; i++)
            //{
            //    RepTest test = new RepTest()
            //    {
            //        Account = Account,
            //        Date = DateTime.Now,
            //        Number = (short)RepeatabilityTests.Count,
            //        ReferenceValue = SelectedCalibration.Repeatability.ReferenceValue,
            //        Status = true,
            //        Measurements = new HashSet<RepTest.Measurement>()
            //    {
            //        new RepTest.Measurement()
            //        {
            //            Result = 200.02f
            //        },
            //        new RepTest.Measurement()
            //        {
            //            Result = 200.02f
            //        },
            //        new RepTest.Measurement()
            //        {
            //            Result = 200.02f
            //        },
            //        new RepTest.Measurement()
            //        {
            //            Result = 200.02f
            //        },
            //        new RepTest.Measurement()
            //        {
            //            Result = 200.02f
            //        },
            //        new RepTest.Measurement()
            //        {
            //            Result = 200.02f
            //        },
            //        new RepTest.Measurement()
            //        {
            //            Result = 200.02f
            //        },
            //        new RepTest.Measurement()
            //        {
            //            Result = 200.02f
            //        },
            //        new RepTest.Measurement()
            //        {
            //            Result = 200.02f
            //        },
            //        new RepTest.Measurement()
            //        {
            //            Result = 200.02f
            //        },
            //    }
            //    };
            //    SelectedCalibration.Repeatability.ReferenceValue.Tests.Add(test);
            //    RepeatabilityTests.Add(test);
            //}
            //TODO skloni ovo

            DialogViewModel = new NewRepeatabilityTestDialogViewModel(SelectedCalibration.Repeatability.ReferenceValue, Account, this);

            DialogContent = new Views.Scales.Dialogs.NewRepeatabilityTestDialog()
            {
                DataContext = DialogViewModel
            };

            IsDialogOpened = true;
        }

        private bool isRepeatabilityPopupDataGridPrintingOpen;
        //TODO add test edit
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
            if (PrintRepeatabilityDataGridStartTest <= 0 || PrintRepeatabilityDataGridEndTest <= 0)
            {
                MessageQueue.Enqueue("Morate uneti pozivitve brojeve");
                return;
            }

            if (PrintRepeatabilityDataGridEndTest - PrintRepeatabilityDataGridStartTest < 0)
            {
                MessageQueue.Enqueue("Morate izabrati opseg od bar 1 testa");
                return;
            }

            PrintDG.Print<ScaleRepeatabilityTest>(dataGrid, "Tabelarni prikaz testova ponovljivosti vage", string.Format("Vaga: {0}/{1}/{2}", Scale.Manufacturer, Scale.Type, Scale.SerialNumber), string.Format("Opseg: {0}/{1}/{2} | Jedinica: {3}", SelectedRange.UpperValue, SelectedRange.LowerValue, SelectedRange.Graduate, SelectedRange.WeightUnit), string.Format("Broj etaloniranja: {0} | Broj uverenja: {1}", SelectedCalibration.Number, SelectedCalibration.Verification.NumberOfVerification));

        }
    }
}
