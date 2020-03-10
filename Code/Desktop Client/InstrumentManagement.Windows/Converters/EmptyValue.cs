namespace InstrumentManagement.Windows.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Converter that converts an empty string value to null value
    /// </summary>
    public class EmptyValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(System.Convert.ToString(value)))
            {
                return null;
            }

            return value;
        }
    }
}
