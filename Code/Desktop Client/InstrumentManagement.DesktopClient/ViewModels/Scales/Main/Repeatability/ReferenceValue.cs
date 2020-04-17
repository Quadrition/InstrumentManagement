namespace InstrumentManagement.DesktopClient.ViewModels.Scales.Main
{
    using InstrumentManagement.Data.Accounts;
    using InstrumentManagement.Data.Scales;
    using InstrumentManagement.DesktopClient.ViewModels.Scales.Dialogs;
    using InstrumentManagement.Windows;
    using System.Collections.Generic;
    using System.Windows.Input;

    public partial class ScaleWindowViewModel
    {
        private ICollection<ScaleWeight> repeatabilityWeights;

        /// <summary>
        /// Gets or sets a list of <see cref="Weight"/> from the <see cref="Data.Scales.Repeatability.ReferenceValue"/>
        /// </summary>
        public ICollection<ScaleWeight> RepeatabilityWeights
        {
            get
            {
                return repeatabilityWeights;
            }
            set
            {
                repeatabilityWeights = value;
                NotifyPropertyChanged(nameof(RepeatabilityWeights));
            }
        }

        private ScaleWeight selectedRepeatabilityWeight;

        /// <summary>
        /// Gets or sets a selected <see cref="ScaleWeight"/> from the <see cref="RepeatabilityWeights"/>
        /// </summary>
        public ScaleWeight SelectedRepeatabilityWeight
        {
            get
            {
                return selectedRepeatabilityWeight;
            }
            set
            {
                selectedRepeatabilityWeight = value;
                NotifyPropertyChanged(nameof(SelectedRepeatabilityWeight));
            }
        }

        /// <summary>
        /// Gets an <see cref="ICommand"/> for opening a <see cref="Views.Scales.Dialogs.NewWeightDialog"/>
        /// </summary>
        public ICommand ShowNewScaleRepeatabilityWeightDialogCommand
        {
            get
            {
                return new ActionCommand(a => ShowNewScaleRepeatabilityWeightDialog(), p => IsLastCalibration == true && Account is Administrator);
            }
        }

        /// <summary>
        /// Opens a <see cref="Views.Scales.Dialogs.NewWeightDialog"/>
        /// </summary>
        private void ShowNewScaleRepeatabilityWeightDialog()
        {
            DialogViewModel = new NewWeightDialogViewModel(this);

            DialogContent = new Views.Scales.Dialogs.NewWeightDialog()
            {
                DataContext = DialogViewModel
            };

            IsDialogOpened = true;
        }

        /// <summary>
        /// Gets an <see cref="ICommand"/> for removing a <see cref="SelectedRepeatabilityWeight"/> from the <see cref="RepeatabilityWeights"/>
        /// </summary>
        public ICommand RemoveScaleRepeatabilityWeightDialogCommand
        {
            get
            {
                return new ActionCommand(a => RemoveScaleRepeatabilityWeightDialog(), p => SelectedRepeatabilityWeight != null && IsLastCalibration == true && Account is Administrator);
            }
        }

        /// <summary>
        /// Removes a <see cref="SelectedRepeatabilityWeight"/> from the <see cref="RepeatabilityWeights"/>
        /// </summary>
        private void RemoveScaleRepeatabilityWeightDialog()
        {
            SelectedCalibration.Repeatability.ReferenceValue.Weights.Remove(SelectedRepeatabilityWeight);
            RepeatabilityWeights.Remove(SelectedRepeatabilityWeight);
            context.UpdateScale(Scale);

            MessageQueue.Enqueue("Uspešno ste uklonili teg");
        }
    }
}
