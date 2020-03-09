namespace InstrumentManagement.Data.Scales.Accuracy
{
    using InstrumentManagement.Data.Scales.Calibration;
    using InstrumentManagement.Windows;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    /// <summary>
    /// Represents a various periods of tests
    /// </summary>
    public enum TestPeriodicity
    {
        daily = 0,
        weekly = 1,
        biweekly = 2,
        monthly = 3,
        yearly = 4
    }

    /// <summary>
    /// Represents a reference value of <see cref="ScaleCalibrationAccuracy"/>
    /// </summary>
    public partial class ScaleAccuracyReferenceValue : Model
    {
        private TestPeriodicity testPeriodicity;

        /// <summary>
        /// Gets or sets an identification and key to which the <see cref="ScaleAccuracyReferenceValue"/> belongs
        /// </summary>
        [Key, ForeignKey("Accuracy")]
        public override int Id { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="TestPeriodicity"/> for the <see cref="ScaleAccuracyReferenceValue"/>
        /// </summary>
        [Required(ErrorMessage = "Periodičnost testa je obavezan")]
        public TestPeriodicity TestPeriodicity
        {
            get
            {
                return testPeriodicity;
            }
            set
            {
                testPeriodicity = value;
                NotifyPropertyChanged(nameof(TestPeriodicity));
            }
        }

        /// <summary>
        /// Gets or sets a <see cref="ScaleCalibrationAccuracy"/> to which the <see cref="ScaleAccuracyReferenceValue"/> belong
        /// </summary>
        public virtual ScaleCalibrationAccuracy Accuracy { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="ScaleAccuracyReferenceValueMeasurement"/> for the <see cref="ScaleAccuracyReferenceValue"/>
        /// </summary>
        public virtual ICollection<ScaleAccuracyReferenceValueMeasurement> Measurements { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="ScaleAccuracyTest"/> for the <see cref="ScaleAccuracyReferenceValue"/>
        /// </summary> 
        public virtual ICollection<ScaleAccuracyTest> Tests { get; set; }

        public static double Coefficient = 2.4f;

        /// <summary>
        /// Initializes a new instance of <see cref="ScaleAccuracyReferenceValue"/> class
        /// </summary>
        public ScaleAccuracyReferenceValue()
        {
            Measurements = new HashSet<ScaleAccuracyReferenceValueMeasurement>();
            Tests = new HashSet<ScaleAccuracyTest>();
        }

        static string[] ValidatedProperties = { "TestPeriodicity" };

        /// <summary>
        /// Checks if all <see cref="ScaleAccuracyReferenceValue"/>'s properties are valid
        /// </summary>
        /// <returns>True if all properties is valid, otherwise false</returns>
        public override bool IsValid
        {
            get
            {
                foreach (ScaleAccuracyReferenceValueMeasurement measurement in Measurements)
                {
                    if (!measurement.IsValid)
                        return false;
                }

                return ValidatedProperties.FirstOrDefault(perp => OnValidate(perp) != null) == null;
            }
        }

    }

    /// <summary>
    /// Represents a measurement of the <see cref="ScaleAccuracyReferenceValue"/>
    /// </summary>
    public class ScaleAccuracyReferenceValueMeasurement : Model
    {
        private double checkPoint;
        private double referenceValue;

        /// <summary>
        /// Gets or sets a check point for the <see cref="ScaleAccuracyReferenceValueMeasurement"/>
        /// </summary>
        [Required(ErrorMessage = "Tačka provere je obavezna"), Index("IX_ScaleAccuracyReferenceValueMeasurementCheckPointAndAccuracyReferenceValueId", 1, IsUnique = true)]
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
        /// Gets a standard deviation of <see cref="Results"/>
        /// </summary>
        public double AverageResult
        {
            get
            {
                return Results.Select(result => result.Result).Average();
            }
        }

        /// <summary>
        /// Gets or sets a reference value for the <see cref="ScaleAccuracyReferenceValueMeasurement"/>
        /// </summary>
        [Required(ErrorMessage = "Referentna vrednost je obavezna")]
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
        /// Gets a max valid resut for the test to pass
        /// </summary>
        public double MaxValidValue
        {
            get
            {
                return AverageResult + ScaleAccuracyReferenceValue.Coefficient * AccuracyReferenceValue.Accuracy.Calibration.Repeatability.ReferenceValue.ReferenceValue;
            }
        }

        /// <summary>
        /// Gets a min valid resut for the test to pass
        /// </summary>
        public double MinValidValue
        {
            get
            {
                return AverageResult - ScaleAccuracyReferenceValue.Coefficient * AccuracyReferenceValue.Accuracy.Calibration.Repeatability.ReferenceValue.ReferenceValue;
            }
        }

        [Required, ForeignKey("AccuracyReferenceValue"), Index("IX_ScaleAccuracyReferenceValueMeasurementCheckPointAndAccuracyReferenceValueId", 2, IsUnique = true)]
        public int AccuracyReferenceValueId { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="ScaleAccuracyReferenceValue"/> to which the <see cref="ScaleAccuracyReferenceValueMeasurement"/> belongs
        /// </summary>
        public virtual ScaleAccuracyReferenceValue AccuracyReferenceValue { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="ScaleAccuracyReferenceValueMeasurementResult"/> for the <see cref="ScaleAccuracyReferenceValueMeasurement"/>
        /// </summary>
        public virtual ICollection<ScaleAccuracyReferenceValueMeasurementResult> Results { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="TestMeasurement"/> for the <see cref="AccRefValMeasurement"/>
        /// </summary>
        public virtual ICollection<ScaleAccuracyTestMeasurement> TestMeasurements { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="ScaleWeight"/> used for the <see cref="ScaleAccuracyReferenceValueMeasurement"/>
        /// </summary>
        public virtual ICollection<ScaleWeight> Weights { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ScaleAccuracyReferenceValueMeasurement"/> class
        /// </summary>
        public ScaleAccuracyReferenceValueMeasurement()
        {
            Results = new HashSet<ScaleAccuracyReferenceValueMeasurementResult>();
            TestMeasurements = new HashSet<ScaleAccuracyTestMeasurement>();
            Weights = new HashSet<ScaleWeight>();
        }

        /// <summary>
        /// Calculates reference value based on <see cref="CheckPoint"/> and <see cref="Results"/>
        /// </summary>
        public void CalculateReferenceValue()
        {
            ScaleCalibrationAccuracyMeasurement measurement = AccuracyReferenceValue.Accuracy.Measurements.SingleOrDefault(measrement => measrement.CheckPoint == CheckPoint);

            double deviation = Math.Abs(AverageResult - CheckPoint);

            if (measurement == null)
            {
                ReferenceValue = deviation;
            }
            else
            {
                if (Math.Abs(deviation - measurement.Deviation) <= ScaleAccuracyReferenceValue.Coefficient * AccuracyReferenceValue.Accuracy.Calibration.Repeatability.ReferenceValue.ReferenceValue)
                {
                    ReferenceValue = measurement.Deviation;
                }
                else
                {
                    ReferenceValue = deviation;
                }
            }
        }

        static readonly string[] ValidatedProperties = { "CheckPoint" };

        /// <summary>
        /// Checks if all <see cref="ScaleAccuracyReferenceValueMeasurement"/>'s properties are valid
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
    /// Represents a result from the <see cref="ScaleAccuracyReferenceValueMeasurement"/>
    /// </summary>
    public class ScaleAccuracyReferenceValueMeasurementResult : Model
    {
        private double result;

        /// <summary>
        /// Gets or sets a result for the <see cref="ScaleAccuracyReferenceValueMeasurementResult"/>
        /// </summary>
        [Required(ErrorMessage = "Rezultat je obavezan")]
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

        [Required, ForeignKey("Measurement")]
        public int MeasurementId { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="Measurement"/> to which the <see cref="ScaleAccuracyReferenceValueMeasurementResult"/> belongs
        /// </summary>
        public virtual ScaleAccuracyReferenceValueMeasurement Measurement { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ScaleAccuracyReferenceValueMeasurementResult"/> class
        /// </summary>
        public ScaleAccuracyReferenceValueMeasurementResult()
        {

        }

        static readonly string[] ValidatedProperties = { "Result" };

        /// <summary>
        /// Checks if all Sc<see cref="ScaleAccuracyReferenceValueMeasurementResult"/>'s properties are valid
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