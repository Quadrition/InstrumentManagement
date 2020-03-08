namespace InstrumentManagement.Data.Scales.Calibration
{
    using InstrumentManagement.Data.Scales.Repeatability;
    using InstrumentManagement.Windows;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    /// <summary>
    /// Represents a repeability as part of the <see cref="ScaleCalibration"/>
    /// </summary>
    public partial class ScaleCalibrationRepeatability : Model
    {
        private short numberOfRepeats;
        private double nominalMass;
        private double standardDeviation;

        private ScaleRepeatabilityReferenceValue referenceValue;

        /// <summary>
        /// Gets or sets an identification and a key to which the <see cref="ScaleCalibrationRepeatability"/> belongs
        /// </summary>
        [Key, ForeignKey("Calibration")]
        public override int Id { get; set; }

        /// <summary>
        /// Gets or sets a number of repeats for the <see cref="ScaleCalibrationRepeatability"/>
        /// </summary>
        [Required(ErrorMessage = "Broj ponavljanja je obavezan")]
        public short NumberOfRepeats
        {
            get
            {
                return numberOfRepeats;
            }
            set
            {
                numberOfRepeats = value;
                NotifyPropertyChanged(nameof(NumberOfRepeats));
            }
        }

        /// <summary>
        /// Gets or sets a check point for the <see cref="ScaleCalibrationRepeatability"/>
        /// </summary>
        [Required(ErrorMessage = "Nominalna masa je obavezna")]
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

        /// <summary>
        /// Gets or sets a standard deviation for the <see cref="ScaleCalibrationRepeatability"/>
        /// </summary>
        [Required(ErrorMessage = "Standardna devijacija je obavezna")]
        public double StandardDeviation
        {
            get
            {
                return standardDeviation;
            }
            set
            {
                standardDeviation = value;
                NotifyPropertyChanged(nameof(StandardDeviation));
            }
        }

        /// <summary>
        /// Gets or sets a <see cref="ScaleCalibration"/> to which the <see cref="ScaleCalibrationRepeatability"/> belongs
        /// </summary>
        public virtual ScaleCalibration Calibration { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="ScaleRepeatabilityReferenceValue"/> for the <see cref="ScaleRepeatabilityReferenceValue"/>
        /// </summary>
        public virtual ScaleRepeatabilityReferenceValue ReferenceValue
        {
            get
            {
                return referenceValue;
            }
            set
            {
                referenceValue = value;
                NotifyPropertyChanged(nameof(ReferenceValue));
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ScaleCalibrationRepeatability"/> class
        /// </summary>
        public ScaleCalibrationRepeatability()
        {

        }

        /// <summary>
        /// Checks if a property is valid
        /// </summary>
        /// <param name="propertyName">A property to be validated</param>
        /// <returns>True if the property is valid, otherwise false</returns>
        protected override string OnValidate(string propertyName)
        {
            if (propertyName == "NumberOfRepeats")
            {
                if (NumberOfRepeats != 6 && NumberOfRepeats != 10)
                {
                    return "Broj ponavaljanja mora da bude 6 ili 10";
                }
            }

            return base.OnValidate(propertyName);
        }

        static readonly string[] ValidatedProperties = { "NumberOfRepeats", "NominalMass", "StandardDeviation" };

        /// <summary>
        /// Checks if all <see cref="ScaleCalibrationRepeatability"/>'s properties are valid
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
