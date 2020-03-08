namespace InstrumentManagement.Data.Scales
{
    using InstrumentManagement.Data.Scales.Repeatability;
    using InstrumentManagement.Windows;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    /// <summary>
    /// Represents a weight used for the <see cref="Scale"/> tests
    /// </summary>
    public class Weight : Model
    {
        private string manufacturer;
        private string weightClass;
        private string calibration;
        private string serialNumber;
        private double nominalMass;

        /// <summary>
        /// Gets or sets a manufacturer for the <see cref="Weight"/>
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
        /// Get sor sets a class for the <see cref="Weight"/>
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
        /// Gets or sets a calibration for the <see cref="Weight"/>
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
        /// Gets or sets a serial number for the <see cref="Weight"/>
        /// </summary>
        [Index("IX_SerialNumber", IsUnique = true)]
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
        /// Gets or sets a nominal mass for the <see cref="Weight"/>
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

        /// <summary>
        /// Initializes a new instance of <see cref="Weight"/> class
        /// </summary>
        public Weight()
        {

        }

        static readonly string[] ValidatedProperties = { "Manufacturer", "WeightClass", "Calibration", "SerialNumber", "NominalMass" };

        /// <summary>
        /// Checks if all <see cref="Weight"/>'s properties are valid
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
