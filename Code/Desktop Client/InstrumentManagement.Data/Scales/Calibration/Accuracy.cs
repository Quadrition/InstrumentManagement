namespace InstrumentManagement.Data.Scales.Calibration
{
    using InstrumentManagement.Windows;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    /// <summary>
    /// Represents an accuracy as part of the <see cref="ScaleCalibration"/>
    /// </summary>
    public class ScaleCalibrationAccuracy : Model
    {
        /// <summary>
        /// Gets or sets an identification and key to which the <see cref="ScaleCalibrationAccuracy"/> belongs
        /// </summary>
        [Key, ForeignKey("Calibration")]
        public override int Id { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="ScaleCalibration"/> to which the <see cref="ScaleCalibrationAccuracy"/> belongs
        /// </summary>
        public virtual ScaleCalibration Calibration { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="ScaleCalibrationAccuracyMeasurement"/> for the <see cref="ScaleCalibrationAccuracy"/>
        /// </summary>
        public virtual ICollection<ScaleCalibrationAccuracyMeasurement> Measurements { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ScaleCalibrationAccuracy"/> class
        /// </summary>
        public ScaleCalibrationAccuracy()
        {

        }
    }

    /// <summary>
    /// Represents one measurement in the <see cref="ScaleCalibrationAccuracy"/>
    /// </summary>
    public class ScaleCalibrationAccuracyMeasurement : Model
    {
        private double checkPoint;
        private double deviation;

        /// <summary>
        /// Gets or sets a check point for the <see cref="ScaleCalibrationAccuracyMeasurement"/>
        /// </summary>
        [Required(ErrorMessage = "Tačka provere je obavezna"), Index("IX_CheckPointAndAccuracyId", 1, IsUnique = true)]
        [Range(0, double.MaxValue, ErrorMessage = "Tačka provere je neispravna")]
        public double CheckPoint
        {
            get
            {
                return checkPoint;
            }
            set
            {
                checkPoint = value;
                NotifyPropertyChanged(nameof(CheckPoint));
            }
        }

        /// <summary>
        /// Gets or sets a deviation for the <see cref="ScaleCalibrationAccuracyMeasurement"/>
        /// </summary>
        [Required(ErrorMessage = "Odstupanje je obavezno")]
        public double Deviation
        {
            get
            {
                return deviation;
            }
            set
            {
                deviation = value;
                NotifyPropertyChanged(nameof(Deviation));
            }
        }

        [Required, ForeignKey("Accuracy"), Index("IX_CheckPointAndAccuracyId", 2, IsUnique = true)]
        public int AccuracyId { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="Scales.Calibration.ScaleCalibrationAccuracy"/> to which the <see cref="ScaleCalibrationAccuracyMeasurement"/> belongs
        /// </summary>
        public virtual ScaleCalibrationAccuracy Accuracy { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ScaleCalibrationAccuracyMeasurement"/> class
        /// </summary>
        public ScaleCalibrationAccuracyMeasurement()
        {

        }

        static readonly string[] ValidatedProperties = { "CheckPoint", "Deviation" };

        /// <summary>
        /// Checks if all <see cref="ScaleCalibrationAccuracyMeasurement"/>'s properties are valid
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
