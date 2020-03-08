namespace InstrumentManagement.Data.Scales.Calibration
{
    using InstrumentManagement.Windows;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    /// <summary>
    /// Represents a verification for the <see cref="Data.Scales.Calibration.Calibration"/>
    /// </summary>
    public class ScaleVerification : Model
    {
        private string numberOfVerification;

        /// <summary>
        /// Gets or sets an identification and key to which the <see cref="ScaleVerification"/> belongs
        /// </summary>
        [Key, ForeignKey("Calibration")]
        public override int Id { get; set; }

        /// <summary>
        /// Gets or sets a number for the <see cref="ScaleVerification"/>
        /// </summary>
        [StringLength(32, ErrorMessage = "Broj uverenja može imati najviše 32 karaktera")]
        public string NumberOfVerification
        {
            get
            {
                return numberOfVerification;
            }
            set
            {
                numberOfVerification = value;
                NotifyPropertyChanged(nameof(NumberOfVerification));
            }
        }

        /// <summary>
        /// Gets or sets a <see cref="ScaleCalibration"/> to which the <see cref="ScaleVerification"/> belongs
        /// </summary>
        public virtual ScaleCalibration Calibration { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ScaleVerification"/> class
        /// </summary>
        public ScaleVerification()
        {

        }

        static string[] ValidatedProperties = { "NumberOfVerification" };

        /// <summary>
        /// Checks if all <see cref="ScaleVerification"/>'s properties are valid
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
