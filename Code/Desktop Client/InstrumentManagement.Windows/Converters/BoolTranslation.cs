namespace InstrumentManagement.Windows.Converters
{
    using System.Windows.Data;
    using System;
    using System.Globalization;

    /// <summary>
    /// Converter that converts a boolean value to serbian language
    /// </summary>
    public class BoolTranslationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool booleanValue = (bool)value;

            if (booleanValue == true)
            {
                return "Prošao";
            }
            else
            {
                return "Nije prošao";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
