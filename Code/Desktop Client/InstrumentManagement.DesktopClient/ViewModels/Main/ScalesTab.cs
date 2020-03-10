namespace InstrumentManagement.DesktopClient.ViewModels.Main
{
    using InstrumentManagement.Data.Scales;
    using InstrumentManagement.Windows;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public partial class MainWindowViewModel
    {
        private ICollection<Scale> scales;

        /// <summary>
        /// Gets or sets list of <see cref="Scale"/>
        /// </summary>
        public ICollection<Scale> Scales
        {
            get
            {
                return scales;
            }
            set
            {
                scales = value;

                if (value == null || value.Count == 0)
                {
                    ShowScalesVisibility = Visibility.Collapsed;
                }
                else
                {
                    ShowScalesVisibility = Visibility.Visible;
                }

                NotifyPropertyChanged(nameof(Scales));
            }
        }

        private Scale selectedScale;

        /// <summary>
        /// Gets or sets a selected scale from the <see cref="Scales"/>
        /// </summary>
        public Scale SelectedScale
        {
            get
            {
                return selectedScale;
            }
            set
            {
                selectedScale = value;
                NotifyPropertyChanged(nameof(SelectedScale));
            }
        }

        private Visibility showScalesVisibility;

        /// <summary>
        /// Gets or sets a <see cref="Visibility"/> of the scales tab
        /// </summary>
        public Visibility ShowScalesVisibility
        {
            get
            {
                return showScalesVisibility;
            }
            set
            {
                showScalesVisibility = value;
                NotifyPropertyChanged(nameof(ShowScalesVisibility));
            }
        }

        private DataGridRowDetailsVisibilityMode scalesDataGridRowDetailsVisibility;

        /// <summary>
        /// Gets or sets a <see cref="DataGridRowDetailsVisibilityMode"/> of the scales datagrid
        /// </summary>
        public DataGridRowDetailsVisibilityMode ScalesDataGridRowDetailsVisibility
        {
            get
            {
                return scalesDataGridRowDetailsVisibility;
            }
            set
            {
                scalesDataGridRowDetailsVisibility = value;
                NotifyPropertyChanged(nameof(ScalesDataGridRowDetailsVisibility));
            }
        }

        /// <summary>
        /// Gets a <see cref="ICommand"/> for changing the <see cref="ScalesDataGridRowDetailsVisibility"/>
        /// </summary>
        public ICommand ScalesDataGridRowDetailVisibilityCommand
        {
            get
            {
                return new ActionCommand(a => ChangeScalesDataGridRowDetailVisibility());
            }
        }

        /// <summary>
        /// Changes the <see cref="ScalesDataGridRowDetailsVisibility"/>
        /// </summary>
        private void ChangeScalesDataGridRowDetailVisibility()
        {
            switch (ScalesDataGridRowDetailsVisibility)
            {
                case DataGridRowDetailsVisibilityMode.VisibleWhenSelected:
                    ScalesDataGridRowDetailsVisibility = DataGridRowDetailsVisibilityMode.Collapsed;
                    break;

                case DataGridRowDetailsVisibilityMode.Collapsed:
                    ScalesDataGridRowDetailsVisibility = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
                    break;
            }
        }
    }
}
