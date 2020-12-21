namespace InstrumentManagement.Windows.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Markup;

    /// <summary>
    /// Converter that converts a data grid row to it's index value
    /// </summary>
    public class DataGridRowToIndexConverter : MarkupExtension, IValueConverter
    {
        static DataGridRowToIndexConverter converter;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DataGridRow row)
                return row.GetIndex() + 1;
            else
                return -1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (converter == null)
                converter = new DataGridRowToIndexConverter();

            return converter;
        }

        public DataGridRowToIndexConverter()
        {

        }
    }
}
