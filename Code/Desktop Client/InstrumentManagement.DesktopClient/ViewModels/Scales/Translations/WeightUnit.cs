namespace InstrumentManagement.DesktopClient.ViewModels.Scales.Translations
{
    using InstrumentManagement.Data.Scales;
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class WeightUnitConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == "")
                return value;

            WeightUnit weightUnit = (WeightUnit)value;

            switch (weightUnit)
            {
                case WeightUnit.gram:
                    return "Gram";

                case WeightUnit.milligram:
                    return "Miligram";

                default:
                    return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
