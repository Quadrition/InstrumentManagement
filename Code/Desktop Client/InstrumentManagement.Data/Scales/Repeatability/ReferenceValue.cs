namespace InstrumentManagement.Data.Scales.Repeatability
{
    using InstrumentManagement.Windows;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Collections.Generic;
    using System;
    using InstrumentManagement.Data.Scales.Calibration;

    /// <summary>
    /// Represents a reference value of the <see cref="ScaleCalibrationRepeatability"/>
    /// </summary>
    public partial class ScaleRepeatabilityReferenceValue : Model
    {
        private double referenceValue;
        private double nominalMass;
        private int numberOfMeasurements;

        /// <summary>
        /// Gets or sets an identification and key to which the <see cref="ScaleRepeatabilityReferenceValue"/> belongs
        /// </summary> 
        [Key, ForeignKey("Repeatability")]
        public override int Id { get; set; }

        /// <summary>
        /// Gets or sets a reference value for the <see cref="ScaleRepeatabilityReferenceValue"/>
        /// </summary>
        [Required(ErrorMessage = "Referentna vrednost je obavezna")]
        [Range(0, double.MaxValue, ErrorMessage = "Neispravna referentna vrednost")]
        public double ReferenceValue
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
        /// Gets or sets a nominal mass for the <see cref="ScaleRepeatabilityReferenceValue"/>
        /// </summary>
        [Required(ErrorMessage = "Nominalna vrednost je obavezan")]
        [Range(0, double.MaxValue, ErrorMessage = "Neispravna nominalna vrednost")]
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
        /// Gets or sets a number of <see cref="ScaleRepeatabilityReferenceValueMeasurement"/> in reference value
        /// </summary>
        public int NumberOfMeasurements
        {
            get
            {
                return numberOfMeasurements;
            }
            set
            {
                numberOfMeasurements = value;
                NotifyPropertyChanged(nameof(NumberOfMeasurements));
            }
        }

        /// <summary>
        /// Gets a standard deviation of the <see cref="Measurements"/>
        /// </summary>
        public double? StandardDeviation
        {
            get
            {
                if (Measurements != null)
                {
                    return Calculations.StandardDeviation(Measurements.Select(measurement => (double)measurement.Result));
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets a coefficient value for calculating the test results
        /// </summary>
        public double Coefficient
        {
            get
            {
                return NumberOfMeasurements == 10 ? 1.8f : 2.25f;
            }
        }

        /// <summary>
        /// Gets a max value for the valid test
        /// </summary>
        public double MaxValidValue
        {
            get
            {
                return ReferenceValue * Coefficient;
            }
        }

        /// <summary>
        /// Gets or sets a <see cref="ScaleCalibrationRepeatability"/> to which the <see cref="ScaleRepeatabilityReferenceValue"/> belongs
        /// </summary>
        public virtual ScaleCalibrationRepeatability Repeatability { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="ScaleRepeatabilityReferenceValueMeasurement"/> for the <see cref="ScaleRepeatabilityReferenceValue"/>
        /// </summary>
        public virtual ICollection<ScaleRepeatabilityReferenceValueMeasurement> Measurements { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ScaleRepeatabilityReferenceValue"/> class
        /// </summary>
        public ScaleRepeatabilityReferenceValue()
        {
            Measurements = new HashSet<ScaleRepeatabilityReferenceValueMeasurement>();
        }

        /// <summary>
        /// Calculates the <see cref="ReferenceValue"/> based on inputed <see cref="NominalMass"/> and <see cref="StandardDeviation"/>
        /// </summary>
        public void CalculateReferenceValue()
        {
            // Check if standard deviation exists
            if (StandardDeviation == null)
            {
                throw new NotImplementedException();
            }

            if (NominalMass == Repeatability.NominalMass)
            {
                if (StandardDeviation <= Coefficient * Repeatability.StandardDeviation)
                {
                    ReferenceValue = Repeatability.StandardDeviation;
                }

                else
                {
                    ReferenceValue = (double)StandardDeviation;
                }
            }
            else
            {
                ReferenceValue = (double)StandardDeviation;
            }
        }

        /// <summary>
        /// Checks if a property is valid
        /// </summary>
        /// <param name="propertyName">A property to be validated</param>
        /// <returns>True if the property is valid, otherwise false</returns>
        protected override string OnValidate(string propertyName)
        {
            if (propertyName == "NumberOfMeasurements")
            {
                if (NumberOfMeasurements != 6 && NumberOfMeasurements != 10)
                {
                    return "Broj ponavljanja mora biti 6 ili 10";
                }
            }

            return base.OnValidate(propertyName);
        }

        static readonly string[] ValidatedProperties = { "ReferenceValue", "NominalMass" };

        /// <summary>
        /// Checks if all <see cref="ScaleRepeatabilityReferenceValue"/>'s properties are valid
        /// </summary>
        /// <returns>Validation result</returns>
        public override bool IsValid
        {
            get
            {
                return ValidatedProperties.FirstOrDefault(perp => OnValidate(perp) != null) == null;
            }
        }
    }

    /// <summary>
    /// Represents a measurement of <see cref="ScaleRepeatabilityReferenceValue"/>
    /// </summary>
    public class ScaleRepeatabilityReferenceValueMeasurement : Model
    {
        private double? result;

        /// <summary>
        /// Gets or sets a result for the <see cref="ScaleRepeatabilityReferenceValueMeasurement"/>
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Rezultat je neispravan")]
        public double? Result
        {
            get
            {
                return result;
            }
            set
            {
                result = value;
                NotifyPropertyChanged(nameof(Result));
            }
        }

        [Required, ForeignKey("ReferenceValue")]
        public int ReferenceValueId { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="ScaleRepeatabilityReferenceValue"/> to which the <see cref="ScaleRepeatabilityReferenceValueMeasurement"/> belongs
        /// </summary>
        public virtual ScaleRepeatabilityReferenceValue ReferenceValue { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ScaleRepeatabilityReferenceValueMeasurement"/> class
        /// </summary>
        public ScaleRepeatabilityReferenceValueMeasurement()
        {

        }

        static readonly string[] ValidatedProperties = { "Result" };

        /// <summary>
        /// Checks if all <see cref="ScaleRepeatabilityReferenceValueMeasurement"/>'s properties are valid
        /// </summary>
        /// <returns>Validation result</returns>
        public override bool IsValid
        {
            get
            {
                return ValidatedProperties.FirstOrDefault(perp => OnValidate(perp) != null) == null;
            }
        }
    }
}