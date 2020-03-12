namespace InstrumentManagement.Data.Scales
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using InstrumentManagement.Windows;
    using System.Collections.Generic;
    using InstrumentManagement.Data.Scales.Calibration;

    /// <summary>
    /// Represents the units used for the weighting purposes
    /// </summary>
    public enum WeightUnit
    {
        gram = 0,
        milligram = 1
    }

    /// <summary>
    /// Represents a range which is related to the <see cref="Scales.Scale"/>
    /// </summary>
    public class ScaleRange : Model
    {
        private double upperValue;
        private double lowerValue;
        private double graduate;

        /// <summary>
        /// Gets or sets an upper range value for the <see cref="ScaleRange"/>
        /// </summary>
        [Required(ErrorMessage = "Gornja granica je obavezna"), Index("IX_ScaleRangeUpperRangeAndScaleId", 1, IsUnique = true)]
        [Range(0, double.MaxValue, ErrorMessage = "Gornja granica opsega je neispravna")]
        public double UpperValue
        {
            get
            {
                return upperValue;
            }
            set
            {
                upperValue = value;
                NotifyPropertyChanged(nameof(UpperValue));
            }
        }

        /// <summary>
        /// Gets or sets a lower range value for the <see cref="ScaleRange"/>
        /// </summary>
        [Required(ErrorMessage = "Donja granica je obavezna")]
        [Range(0, double.MaxValue, ErrorMessage = "Donja granica opsega je neispravna")]
        public double LowerValue
        {
            get
            {
                return lowerValue;
            }
            set
            {
                lowerValue = value;
                NotifyPropertyChanged(nameof(LowerValue));
            }
        }

        /// <summary>
        /// Gets or sets a graduate for the <see cref="ScaleRange"/>
        /// </summary>
        [Required(ErrorMessage = "Podeljak je obavezan")]
        [Range(0, double.MaxValue, ErrorMessage = "Podeljak je neispravan")]
        public double Graduate
        {
            get
            {
                return graduate;
            }
            set
            {
                graduate = value;
                NotifyPropertyChanged(nameof(Graduate));
            }
        }

        /// <summary>
        /// Gets or sets a <see cref="Scales.WeightUnit"/> which is used for representing values of the <see cref="ScaleRange"/> and all related entities
        /// </summary>
        [Required(ErrorMessage = "Merna jedinica je obavezna")]
        public WeightUnit WeightUnit { get; set; }

        [Required, ForeignKey("Scale"), Index("IX_ScaleRangeUpperRangeAndScaleId", 2, IsUnique = true)]
        public int ScaleId { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="Scales.Scale"/> to which the <see cref="ScaleRange"/> is related
        /// </summary>
        public virtual Scale Scale { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="ScaleCalibration"/> which are related to the <see cref="ScaleRange"/>
        /// </summary>
        public virtual ICollection<ScaleCalibration> Calibrations { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ScaleRange"/> class
        /// </summary>
        public ScaleRange()
        {
            Calibrations = new HashSet<ScaleCalibration>();
        }

        /// <summary>
        /// Checks if a property is valid
        /// </summary>
        /// <param name="propertyName">A property to be validated</param>
        /// <returns>True if the property is valid, otherwise false</returns>
        protected override string OnValidate(string propertyName)
        {
            if (propertyName == "UpperValue" || propertyName == "LowerValue")
            {
                if (propertyName == "UpperValue" && UpperValue == 0)
                {
                    return "Gornja granica ne može biti 0";
                }

                if (UpperValue <= LowerValue + Graduate)
                {
                    return "Gornja granica mora biti veća od donje granice";
                }
            }
            else if (propertyName == "Graduate")
            {
                if (Graduate == 0)
                {
                    return "Podeljak ne može biti 0";
                }
            }

            return base.OnValidate(propertyName);
        }

        static readonly string[] ValidatedProperties = { "UpperValue", "LowerValue", "Graduate", "BaseUnit" };

        /// <summary>
        /// Checks if all <see cref="ScaleRange"/>'s properties are valid
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
