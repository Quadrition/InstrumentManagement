namespace InstrumentManagement.DesktopClient.ViewModels.Scales.Main
{
    using InstrumentManagement.Data.Accounts;
    using InstrumentManagement.Data.Scales.Accuracy;
    using InstrumentManagement.DesktopClient.ViewModels.Scales.Dialogs;
    using InstrumentManagement.Windows;
    using System.Collections.Generic;
    using System.Windows.Controls;
    using System.Windows.Input;

    public partial class ScaleWindowViewModel
    {
        private ICollection<ScaleAccuracyTest> accuracyTests;

        /// <summary>
        /// Gets or sets a list of <see cref="ScaleAccuracyTest"/>
        /// </summary>
        public ICollection<ScaleAccuracyTest> AccuracyTests
        {
            get
            {
                return accuracyTests;
            }
            set
            {
                accuracyTests = value;
                NotifyPropertyChanged(nameof(AccuracyTests));
            }
        }
        //TODO add test edit
        /// <summary>
        /// Gets a command for opening a <see cref="Views.Scales.Dialogs.NewAccuracyTestDialog"/>
        /// </summary>
        public ICommand ShowNewScaleAccuracyTestDialogCommand
        {
            get
            {
                return new ActionCommand(a => ShowNewScaleAccuracyTestDialog(), p => IsLastCalibration == true && SelectedCalibration.Accuracy.ReferenceValue != null);
            }
        }

        private ScaleAccuracyTest selectedAccuracyTest;

        /// <summary>
        /// Gets or sets a selected <see cref="ScaleAccuracyTest"/> from the <see cref="AccuracyTests"/>
        /// </summary>
        public ScaleAccuracyTest SelectedAccuracyTest
        {
            get
            {
                return selectedAccuracyTest;
            }
            set
            {
                selectedAccuracyTest = value;
                NotifyPropertyChanged(nameof(SelectedAccuracyTest));
            }
        }

        /// <summary>
        /// Opens a <see cref="Views.Scales.Dialogs.NewAccuracyTestDialog"/>
        /// </summary>
        private void ShowNewScaleAccuracyTestDialog()
        {
            DialogViewModel = new NewAccuracyTestDialogViewModel(SelectedCalibration.Accuracy.ReferenceValue, Account, this);

            DialogContent = new Views.Scales.Dialogs.NewAccuracyTestDialog()
            {
                DataContext = DialogViewModel
            };

            IsDialogOpened = true;
        }

        private DataGridRowDetailsVisibilityMode accuracyTestsDataGridRowDetailsVisibility;

        /// <summary>
        /// Gets or sets a <see cref="DataGridRowDetailsVisibilityMode"/> for the <see cref="DataGrid"/>
        /// </summary>
        public DataGridRowDetailsVisibilityMode AccuracyTestsGridRowDetailsVisibility
        {
            get
            {
                return accuracyTestsDataGridRowDetailsVisibility;
            }
            set
            {
                accuracyTestsDataGridRowDetailsVisibility = value;
                NotifyPropertyChanged(nameof(AccuracyTestsGridRowDetailsVisibility));
            }
        }

        /// <summary>
        /// Gets an <see cref="ICommand"/> for changing datagrid row detail visibility mode
        /// </summary>
        public ICommand AccuracyTestsGridRowDetailVisibilityCommand
        {
            get
            {
                return new ActionCommand(a => ChangeAccuracyTestsDataGridRowDetailVisibility());
            }
        }

        /// <summary>
        /// Changes datagrid row detail visiblity
        /// </summary>
        private void ChangeAccuracyTestsDataGridRowDetailVisibility()
        {
            switch (AccuracyTestsGridRowDetailsVisibility)
            {
                case DataGridRowDetailsVisibilityMode.VisibleWhenSelected:
                    AccuracyTestsGridRowDetailsVisibility = DataGridRowDetailsVisibilityMode.Collapsed;
                    break;

                case DataGridRowDetailsVisibilityMode.Collapsed:
                    AccuracyTestsGridRowDetailsVisibility = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
                    break;
            }
        }

        private ScaleAccuracyReferenceValueMeasurement selectedAccuracyTestMeasurement;

        /// <summary>
        /// Gets or sets a selected <see cref="TAccuracy.AccReferenceValue.AccRefValMeasurement"/> for test preview
        /// </summary>
        public ScaleAccuracyReferenceValueMeasurement SelectedAccuracyTestMeasurement
        {
            get
            {
                return selectedAccuracyTestMeasurement;
            }
            set
            {
                selectedAccuracyTestMeasurement = value;
                NotifyPropertyChanged(nameof(selectedAccuracyTestMeasurement));
            }
        }

        /// <summary>
        /// Gets an <see cref="ICommand"/> for opening a <see cref="Views.Scales.Dialogs.EditAccuracyTestDialog"/>
        /// </summary>
        public ICommand ShowEditScaleAccuracyTestDialogCommand
        {
            get
            {
                return new ActionCommand(a => ShowEditScaleAccuracyTestDialog(), p => IsLastCalibration == true && Account is Administrator && SelectedAccuracyTest != null);
            }
        }

        /// <summary>
        /// Opens a <see cref="Views.Scales.Dialogs.EditAccuracyTestDialog"/>
        /// </summary>
        private void ShowEditScaleAccuracyTestDialog()
        {
            DialogViewModel = new EditAccuracyTestDialogViewModel(SelectedAccuracyTest, Account, this);

            DialogContent = new Views.Scales.Dialogs.EditAccuracyTestDialog()
            {
                DataContext = DialogViewModel
            };

            IsDialogOpened = true;
        }

        private bool isAccuracyPopupDataGridPrintingOpen;

        /// <summary>
        /// Gets or sets if popup for accuracy data grid printing is opened
        /// </summary>
        public bool IsPopupAccuracyDataGridPrintingOpen
        {
            get
            {
                return isAccuracyPopupDataGridPrintingOpen;
            }
            set
            {
                isAccuracyPopupDataGridPrintingOpen = value;
                NotifyPropertyChanged(nameof(IsPopupAccuracyDataGridPrintingOpen));
            }
        }

        private bool isAccuracyPopupSinglePointDataGridPrintingOpen;

        /// <summary>
        /// Gets or sets if popup for accuracy data grid printing is opened
        /// </summary>
        public bool IsPopupAccuracySinglePointDataGridPrintingOpen
        {
            get
            {
                return isAccuracyPopupSinglePointDataGridPrintingOpen;
            }
            set
            {
                isAccuracyPopupSinglePointDataGridPrintingOpen = value;
                NotifyPropertyChanged(nameof(IsPopupAccuracySinglePointDataGridPrintingOpen));
            }
        }

        /// <summary>
        /// Gets an <see cref="ICommand"/> for opening a popup for accuracy data grid printing
        /// </summary>
        public ICommand OpenPopupAccuracyDataGridPrinting
        {
            get
            {
                return new ActionCommand(a => IsPopupAccuracyDataGridPrintingOpen = true);
            }
        }

        /// <summary>
        /// Gets an <see cref="ICommand"/> for opening a popup for accuracy single point data grid printing
        /// </summary>
        public ICommand OpenPopupAccuracySinglePointDataGridPrinting
        {
            get
            {
                return new ActionCommand(a => IsPopupAccuracySinglePointDataGridPrintingOpen = true);
            }
        }

        private int printAccuracyDataGridStartTest;

        /// <summary>
        /// Gets or sets a start test for printing data grid
        /// </summary>
        public int PrintAccuracyDataGridStartTest
        {
            get
            {
                return printAccuracyDataGridStartTest;
            }
            set
            {
                printAccuracyDataGridStartTest = value;
                NotifyPropertyChanged(nameof(PrintAccuracyDataGridStartTest));
            }
        }

        private int printAccuracyDataGridEndTest;

        /// <summary>
        /// Gets or sets a end test for printing data grid
        /// </summary>
        public int PrintAccuracyDataGridEndTest
        {
            get
            {
                return printAccuracyDataGridEndTest;
            }
            set
            {
                printAccuracyDataGridEndTest = value;
                NotifyPropertyChanged(nameof(PrintAccuracyDataGridEndTest));
            }
        }

        /// <summary>
        /// Prints a accuracy data grid
        /// </summary>
        public void PrintAccuracyDataGrid(DataGrid dataGrid, bool singlePoint)
        {
            if (PrintAccuracyDataGridStartTest <= 0 || PrintAccuracyDataGridEndTest <= 0)
            {
                MessageQueue.Enqueue("Morate uneti pozivitve brojeve");
                return;
            }

            if (PrintAccuracyDataGridEndTest - PrintAccuracyDataGridStartTest < 0)
            {
                MessageQueue.Enqueue("Morate izabrati opseg od bar 1 testa");
                return;
            }

            if (singlePoint)
            {
                PrintDG.Print<ScaleAccuracyTestMeasurement>(dataGrid, "Tabelarni prikaz testova tačnosti vage", string.Format("Vaga: {0}/{1}/{2}", Scale.Manufacturer, Scale.Type, Scale.SerialNumber), string.Format("Opseg: {0}/{1}/{2} | Jedinica: {3}", SelectedRange.UpperValue, SelectedRange.LowerValue, SelectedRange.Graduate, SelectedRange.WeightUnit), string.Format("Broj etaloniranja: {0} | Broj uverenja: {1}", SelectedCalibration.Number, SelectedCalibration.Verification.NumberOfVerification), string.Format("Tačka provere: {0}", SelectedAccuracyReferenceValueMeasurement.CheckPoint));
            }
            else
            {
                PrintDG.Print<ScaleAccuracyTest>(dataGrid, "Tabelarni prikaz testova tačnosti vage", string.Format("Vaga: {0}/{1}/{2}", Scale.Manufacturer, Scale.Type, Scale.SerialNumber), string.Format("Opseg: {0}/{1}/{2} | Jedinica: {3}", SelectedRange.UpperValue, SelectedRange.LowerValue, SelectedRange.Graduate, SelectedRange.WeightUnit), string.Format("Broj etaloniranja: {0} | Broj uverenja: {1}", SelectedCalibration.Number, SelectedCalibration.Verification.NumberOfVerification));
            }

        }
    }
}
