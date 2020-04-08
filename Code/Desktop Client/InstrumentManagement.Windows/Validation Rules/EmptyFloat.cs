namespace InstrumentManagement.Windows.ValidationRules
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;

    public class EmptyFloatValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string stringValue = Convert.ToString(value);

            if (string.IsNullOrEmpty(stringValue))
            {
                return new ValidationResult(true, null);
            }

            if (!double.TryParse(stringValue, out double result))
            {
                return new ValidationResult(false, "Broj je neispravan");
            }

            return new ValidationResult(true, null);
        }
    }
}
