namespace InstrumentManagement.Data.Scales.Repeatability
{
    using InstrumentManagement.Windows;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Collections.Generic;
    using System;
    using InstrumentManagement.Data.Accounts;

    /// <summary>
    /// Represents a test for the <see cref="ScaleRepeatabilityReferenceValue"/>
    /// </summary>
    public class ScaleRepeatabilityTest : Model
    {
        private short number;
        private DateTime date;
        private bool status;

        /// <summary>
        /// Gets or sets a number for the <see cref="ScaleRepeatabilityTest"/>
        /// </summary>
        [Required(ErrorMessage = "Broj testa je obavezan"), Index("IX_NumberAndReferenceValueId", 1, IsUnique = true)]
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
        /// Gets or sets a date for the <see cref="ScaleRepeatabilityTest"/>
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
        /// Gets or sets a status for the <see cref="ScaleRepeatabilityTest"/>
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

        /// <summary>
        /// Gets a standard deviation of the <see cref="Measurements"/>
        /// </summary>
        public double StandardDeviation
        {
            get
            {
                return Calculations.StandardDeviation(Measurements.Select(measurement => measurement.Result));
            }
        }

        [Required, ForeignKey("Account")]
        public int AccountId { get; set; }

        /// <summary>
        /// Gets or sets an <see cref="Account"/> that did the test
        /// </summary>
        public virtual Account Account { get; set; }

        [Required, ForeignKey("ReferenceValue")]
        public int ReferenceValueId { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="ScaleRepeatabilityReferenceValue"/> to which the <see cref="ScaleRepeatabilityTest"/> belongs
        /// </summary>
        [Index("IX_NumberAndReferenceValueId", 2, IsUnique = true)]
        public virtual ScaleRepeatabilityReferenceValue ReferenceValue { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="ScaleRepeatabilityTestMeasurement"/>
        /// </summary>
        public virtual ICollection<ScaleRepeatabilityTestMeasurement> Measurements { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ScaleRepeatabilityTest"/> class
        /// </summary>
        public ScaleRepeatabilityTest()
        {
            Measurements = new HashSet<ScaleRepeatabilityTestMeasurement>();
        }

        /// <summary>
        /// Calculates a status of the <see cref="ScaleRepeatabilityTest"/> based on inputed <see cref="Measurements"/>
        /// </summary>
        public void CalculateResults()
        {
            if (StandardDeviation <= ReferenceValue.MaxValidValue)
            {
                Status = true;
            }
            else
            {
                Status = false;
            }
        }

        static readonly string[] ValidatedProperties = { "Number", "Date", "Status" };

        /// <summary>
        /// Checks if all <see cref="ScaleRepeatabilityTest"/>'s properties are valid
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
    /// Represents a measurement of the <see cref="ScaleRepeatabilityTest"/>
    /// </summary>
    public class ScaleRepeatabilityTestMeasurement : Model
    {
        private double result;

        /// <summary>
        /// Gets or sets a result of the <see cref="ScaleRepeatabilityTestMeasurement"/>
        /// </summary>
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

        [Required, ForeignKey("Test")]
        public int TestId { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="Repeatability.ScaleRepeatabilityTest"/> to which the <see cref="ScaleRepeatabilityTestMeasurement"/> belongs
        /// </summary>
        public virtual ScaleRepeatabilityTest Test { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ScaleRepeatabilityTestMeasurement"/> class
        /// </summary>
        public ScaleRepeatabilityTestMeasurement()
        {

        }

        static readonly string[] ValidatedProperties = { "Result" };

        /// <summary>
        /// Checks if all <see cref="ScaleRepeatabilityTestMeasurement"/>'s properties are valid
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