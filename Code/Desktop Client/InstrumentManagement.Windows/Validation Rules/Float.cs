namespace InstrumentManagement.Windows.ValidationRules
{
    using System.Globalization;
    using System.Windows.Controls;

    /// <summary>
    /// Custom rule which checks if user input is float value
    /// </summary>
    public class FloatValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!float.TryParse(System.Convert.ToString(value), out float result))
            {
                return new ValidationResult(false, "Broj je neispravan");
            }

            return new ValidationResult(true, null);
        }
    }
}
