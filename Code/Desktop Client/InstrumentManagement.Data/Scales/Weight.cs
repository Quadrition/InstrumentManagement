namespace InstrumentManagement.Data.Scales
{
    using InstrumentManagement.Data.Scales.Accuracy;
    using InstrumentManagement.Data.Scales.Repeatability;
    using InstrumentManagement.Windows;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    /// <summary>
    /// Represents a weight used for the <see cref="Scale"/> tests
    /// </summary>
    public class ScaleWeight : Model
    {
        private string manufacturer;
        private string weightClass;
        private string calibration;
        private string serialNumber;
        private double nominalMass;

        /// <summary>
        /// Gets or sets a manufacturer for the <see cref="ScaleWeight"/>
        /// </summary>
        [StringLength(32, ErrorMessage = "Proizvođač može imati najviše 32 karaktera")]
        public string Manufacturer
        {
            get
            {
                return manufacturer;
            }
            set
            {
                manufacturer = value;
                NotifyPropertyChanged(nameof(Manufacturer));
            }
        }

        /// <summary>
        /// Get sor sets a class for the <see cref="ScaleWeight"/>
        /// </summary>
        [StringLength(32, ErrorMessage = "Klasa može imati najviše 32 karaktera")]
        public string WeightClass
        {
            get
            {
                return weightClass;
            }
            set
            {
                weightClass = value;
                NotifyPropertyChanged(nameof(WeightClass));
            }
        }

        /// <summary>
        /// Gets or sets a calibration for the <see cref="ScaleWeight"/>
        /// </summary>
        [StringLength(32, ErrorMessage = "Uverenje može imati najviše 32 karaktera")]
        public string Calibration
        {
            get
            {
                return calibration;
            }
            set
            {
                calibration = value;
                NotifyPropertyChanged(nameof(Calibration));
            }
        }

        /// <summary>
        /// Gets or sets a serial number for the <see cref="ScaleWeight"/>
        /// </summary>
        [Index("IX_WeightSerialNumber", IsUnique = true)]
        [StringLength(32, ErrorMessage = "Serijski broj može imati najviše 32 karaktera")]
        public string SerialNumber
        {
            get
            {
                return serialNumber;
            }
            set
            {
                serialNumber = value;
                NotifyPropertyChanged(nameof(SerialNumber));
            }
        }

        /// <summary>
        /// Gets or sets a nominal mass for the <see cref="ScaleWeight"/>
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Nominalna masa je neispravna")]
        public double NominalMass
        {
            get
            {
                return nominalMass;
            }
            set
            {
                nominalMass = value;
                NotifyPropertyChanged(nameof(NominalMass));
            }
        }

        [ForeignKey("RepeatabilityReferenceValue")]
        public int? RepeatabilityReferenceValueId { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="ScaleRepeatabilityReferenceValue"/> to which the <see cref="ScaleWeight"/> belongs
        /// </summary>
        public virtual ScaleRepeatabilityReferenceValue RepeatabilityReferenceValue { get; set; }

        [ForeignKey("AccuracyReferenceValueMeasurement")]
        public int? AccuracyReferenceValueMeasurementId { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="ScaleAccuracyReferenceValueMeasurement"/> to which the <see cref="ScaleWeight"/> belongs
        /// </summary>
        public virtual ScaleAccuracyReferenceValueMeasurement AccuracyReferenceValueMeasurement { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ScaleWeight"/> class
        /// </summary>
        public ScaleWeight()
        {

        }

        static readonly string[] ValidatedProperties = { "Manufacturer", "WeightClass", "Calibration", "SerialNumber", "NominalMass" };

        /// <summary>
        /// Checks if all <see cref="ScaleWeight"/>'s properties are valid
        /// </summary>
        /// <returns>True if all properties is valid, otherwise false</returns>
        public override bool IsValid
        {
            get
            {
                return ValidatedProperties.FirstOrDefault(perp => OnValidate(perp) != null) == null;
            }
        }
    }
}
