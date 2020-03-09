namespace InstrumentManagement.Data.Scales.Calibration
{
    using InstrumentManagement.Windows;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    /// <summary>
    /// Represents a calibration which is related to the <see cref="Scale"/>
    /// </summary>
    public class ScaleCalibration : Model
    {
        private short number;
        private DateTime? date;
        private short? validFor;
        private string laboratory;

        /// <summary>
        /// Gets or sets a number for the <see cref="ScaleCalibration"/>
        /// </summary>
        [Required(ErrorMessage = "Broj etaloniranja je obavezan"), Index("IX_ScaleCalibrationNumberAndRangeId", 1, IsUnique = true)]
        [Range(1, short.MaxValue, ErrorMessage = "Broj etaloniranja je neispravan")]
        public short Number
        {
            get
            {
                return number;
            }
            set
            {
                number = value;
                NotifyPropertyChanged(nameof(Number));
            }
        }

        /// <summary>
        /// Gets or sets a date for the <see cref="ScaleCalibration"/>
        /// </summary>
        public DateTime? Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
                NotifyPropertyChanged(nameof(Date));
            }
        }

        /// <summary>
        /// Gets or sets for how long will <see cref="ScaleCalibration"/> be valid in months
        /// </summary>
        [Range(1, short.MaxValue, ErrorMessage = "Period važenja etaloniranja je neispravan")]
        public short? ValidFor
        {
            get
            {
                return validFor;
            }
            set
            {
                validFor = value;
                NotifyPropertyChanged(nameof(ValidFor));
            }
        }

        /// <summary>
        /// Gets or sets a laboratory where the <see cref="ScaleCalibration"/> happened
        /// </summary>
        [StringLength(32, ErrorMessage = "Laboratorija može imati najviše 32 karaktera")]
        public string Laboratory
        {
            get
            {
                return laboratory;
            }
            set
            {
                laboratory = value;
                NotifyPropertyChanged(nameof(Laboratory));
            }
        }

        [Required, ForeignKey("Range"), Index("IX_ScaleCalibrationNumberAndRangeId", 2, IsUnique = true)]
        public int RangeId { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="ScaleRange"/> to which the <see cref="ScaleCalibration"/> belongs
        /// </summary>
        public virtual ScaleRange Range { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="Scales.Calibration.ScaleVerification"/> for the <see cref="ScaleCalibration"/>
        /// </summary>
        public virtual ScaleVerification Verification { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="ScaleCalibrationRepeatability"/> that is related to the <see cref="ScaleCalibration"/>
        /// </summary>
        public virtual ScaleCalibrationRepeatability Repeatability { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="ScaleCalibrationAccuracy"/> that is related to the <see cref="ScaleCalibration"/>
        /// </summary>
        public virtual ScaleCalibrationAccuracy Accuracy { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ScaleCalibration"/> class
        /// </summary>
        public ScaleCalibration()
        {

        }

        static readonly string[] ValidatedProperties = { "NumberOfCalibration", "DateOfCalibration", "ValidFor", "Laboratory" };

        /// <summary>
        /// Checks if all <see cref="ScaleCalibration"/>'s properties are valid
        /// </summary>
        /// <returns>True if all properties is valid, otherwise false</returns>
        public override bool IsValid
        {
            get
            {
                return Verification.IsValid && Repeatability.IsValid && Accuracy.IsValid && ValidatedProperties.FirstOrDefault(perp => OnValidate(perp) != null) == null;
            }
        }
    }
}
