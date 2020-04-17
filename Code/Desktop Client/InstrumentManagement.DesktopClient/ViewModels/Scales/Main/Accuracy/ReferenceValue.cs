namespace InstrumentManagement.DesktopClient.ViewModels.Scales.Main
{
    using InstrumentManagement.Data.Accounts;
    using InstrumentManagement.Data.Scales;
    using InstrumentManagement.Data.Scales.Accuracy;
    using InstrumentManagement.DesktopClient.ViewModels.Scales.Dialogs;
    using InstrumentManagement.Windows;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    public partial class ScaleWindowViewModel
    {
        private ScaleAccuracyReferenceValueMeasurement selectedAccuracyReferenceValueMeasurement;

        /// <summary>
        /// Gets or sets a selected <see cref="ScaleAccuracyReferenceValueMeasurement"/> from the <see cref="ScaleAccuracyReferenceValue/>
        /// </summary>
        public ScaleAccuracyReferenceValueMeasurement SelectedAccuracyReferenceValueMeasurement
        {
            get
            {
                return selectedAccuracyReferenceValueMeasurement;
            }
            set
            {
                selectedAccuracyReferenceValueMeasurement = value;
                NotifyPropertyChanged(nameof(SelectedAccuracyReferenceValueMeasurement));

                if (value != null)
                {
                    AccuracyWeights = new ObservableCollection<ScaleWeight>(value.Weights);
                }
            }
        }

        private ICollection<ScaleWeight> accuracyWeights;

        /// <summary>
        /// Gets or sets a list of <see cref="ScaleWeight"/> from the <see cref="ScaleAccuracyReferenceValue"/>
        /// </summary>
        public ICollection<ScaleWeight> AccuracyWeights
        {
            get
            {
                return accuracyWeights;
            }
            set
            {
                accuracyWeights = value;
                NotifyPropertyChanged(nameof(AccuracyWeights));
            }
        }

        private ScaleWeight selectedAccuracyWeight;

        /// <summary>
        /// Gets or sets a selected <see cref="Weight"/> from the <see cref="AccuracyWeights"/>
        /// </summary>
        public ScaleWeight SelectedAccuracyWeight
        {
            get
            {
                return selectedAccuracyWeight;
            }
            set
            {
                selectedAccuracyWeight = value;
                NotifyPropertyChanged(nameof(SelectedAccuracyWeight));
            }
        }

        /// <summary>
        /// Gets an <see cref="ICommand"/> for opening a <see cref="Views.Scales.Dialogs.NewWeightDialog"/>
        /// </summary>
        public ICommand ShowNewScaleAccuracyWeightDialogCommand
        {
            get
            {
                return new ActionCommand(a => ShowNewScaleAccuracyWeightDialog(), p => IsLastCalibration == true && Account is Administrator);
            }
        }

        /// <summary>
        /// Opens a <see cref="Views.NewScaleWeightDialog"/>
        /// </summary>
        private void ShowNewScaleAccuracyWeightDialog()
        {
            DialogViewModel = new NewWeightDialogViewModel(this);

            DialogContent = new Views.Scales.Dialogs.NewWeightDialog()
            {
                DataContext = DialogViewModel
            };

            IsDialogOpened = true;
        }

        /// <summary>
        /// Gets an <see cref="ICommand"/> for removing a <see cref="SelectedAccuracyWeight"/> from the <see cref="AccuracyWeights"/>
        /// </summary>
        public ICommand RemoveScaleAccuracyWeightDialogCommand
        {
            get
            {
                return new ActionCommand(a => RemoveScaleAccuracyWeightDialog(), p => SelectedAccuracyWeight != null && IsLastCalibration == true && Account is Administrator);
            }
        }

        /// <summary>
        /// Removes a <see cref="SelectedAccuracyWeight"/> from the <see cref="AccuracyWeights"/>
        /// </summary>
        private void RemoveScaleAccuracyWeightDialog()
        {
            SelectedAccuracyReferenceValueMeasurement.Weights.Remove(SelectedAccuracyWeight);
            AccuracyWeights.Remove(SelectedAccuracyWeight);
            this.context.UpdateScale(Scale);

            MessageQueue.Enqueue("Uspešno ste uklonili teg");
        }
    }
}
