namespace InstrumentManagement.Data.Scales.Accuracy
{
    using InstrumentManagement.Data.Accounts;
    using InstrumentManagement.Windows;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    /// <summary>
    /// Represents a test for the <see cref="ScaleAccuracyReferenceValue"/>
    /// </summary>
    public class ScaleAccuracyTest : Model
    {
        private short number;
        private DateTime date;

        /// <summary>
        /// Gets or sets a number for the <see cref="ScaleAccuracyTest"/>
        /// </summary>
        [Required(ErrorMessage = "Broj testa je obavezan")/*, Index("IX_NumberAndReferenceValueId", 1, IsUnique = true)*/]
        [Range(1, short.MaxValue, ErrorMessage = "Broj testa je neispravan")]
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
        /// Gets or sets a date for the <see cref="ScaleAccuracyTest"/>
        /// </summary>
        [Required(ErrorMessage = "Datum testa je obavezan")]
        public DateTime Date
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
        /// Gets a status for the whole <see cref="ScaleAccuracyTest"/>
        /// </summary>
        public bool Status
        {
            get
            {
                return Measurements.FirstOrDefault(perp => perp.Status == false) == null;
            }
        }

        [Required, ForeignKey("Account")]
        public int AccountId { get; set; }

        /// <summary>
        /// Gets or sets an <see cref="Accounts.Account"/> that did the test
        /// </summary>
        public virtual Account Account { get; set; }

        [Required, ForeignKey("ReferenceValue")]
        public int ReferenceValueId { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="ScaleAccuracyReferenceValue"/> to which the <see cref="ScaleAccuracyTest"/> belongs
        /// </summary>
        //[Index("IX_NumberAndReferenceValueId", 2, IsUnique = true)]
        public virtual ScaleAccuracyReferenceValue ReferenceValue { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="ScaleAccuracyTestMeasurement"/> for the <see cref="ScaleAccuracyTest"/>
        /// </summary>
        public virtual ICollection<ScaleAccuracyTestMeasurement> Measurements { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ScaleAccuracyTest"/> class
        /// </summary>
        public ScaleAccuracyTest()
        {
            Measurements = new HashSet<ScaleAccuracyTestMeasurement>();
        }

        /// <summary>
        /// Calculates a status of the <see cref="ScaleAccuracyTest"/> based on inputed <see cref="Measurements"/>
        /// </summary>
        public void CalculateResults()
        {
            foreach (ScaleAccuracyTestMeasurement measurement in Measurements)
            {
                if (Math.Abs(measurement.Result - measurement.ReferenceValueMeasurement.CheckPoint) - measurement.ReferenceValueMeasurement.ReferenceValue <= ScaleAccuracyReferenceValue.Coefficient * measurement.ReferenceValueMeasurement.AccuracyReferenceValue.Accuracy.Calibration.Repeatability.ReferenceValue.ReferenceValue)
                {
                    measurement.Status = true;
                }
                else
                {
                    measurement.Status = false;
                }
            }
        }

        static readonly string[] ValidatedProperties = { "Number", "Date" };

        /// <summary>
        /// Checks if all <see cref="ScaleAccuracyTest"/>'s properties are valid
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

    /// <summary>
    /// Represents a measurement of the <see cref="Accuracy.ScaleAccuracyTest"/>
    /// </summary>
    public class ScaleAccuracyTestMeasurement : Model
    {
        private double result;
        private bool status;

        /// <summary>
        /// Gets or sets a result for the <see cref="ScaleAccuracyTestMeasurement"/>
        /// </summary>
        [Required(ErrorMessage = "Rezultat merenja je obavezan")]
        [Range(0, double.MaxValue, ErrorMessage = "Rezultat merenja je neispravan")]
        public double Result
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

        /// <summary>
        /// Gets or sets a status for the <see cref="ScaleAccuracyTestMeasurement"/>
        /// </summary>
        [Required(ErrorMessage = "Status je obavezan")]
        public bool Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                NotifyPropertyChanged(nameof(Status));
            }
        }

        [Required, ForeignKey("Test")]
        public int TestId { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="Accuracy.ScaleAccuracyTest"/> to which the <see cref="ScaleAccuracyTestMeasurement"/> belongs
        /// </summary>
        public virtual ScaleAccuracyTest Test { get; set; }

        [Required, ForeignKey("ReferenceValueMeasurement")]
        public int ReferenceValueMeasurementId { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="ScaleAccuracyReferenceValueMeasurement"/> to which the <see cref="ScaleAccuracyTestMeasurement"/> belongs
        /// </summary>
        public virtual ScaleAccuracyReferenceValueMeasurement ReferenceValueMeasurement { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ScaleAccuracyTestMeasurement"/> class
        /// </summary>
        public ScaleAccuracyTestMeasurement()
        {

        }

        static readonly string[] ValidatedProperties = { "Result" };

        /// <summary>
        /// Checks if all <see cref="ScaleAccuracyTestMeasurement"/>'s properties are valid
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