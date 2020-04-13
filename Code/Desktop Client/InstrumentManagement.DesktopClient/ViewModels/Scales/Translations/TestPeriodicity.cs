namespace InstrumentManagement.DesktopClient.ViewModels.Scales.Translations
{
    using InstrumentManagement.Data.Scales.Accuracy;
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class TestPeriodicityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TestPeriodicity periodicity = (TestPeriodicity)value;

            switch (periodicity)
            {
                case TestPeriodicity.daily:
                    return "Dnevno";

                case TestPeriodicity.weekly:
                    return "Nedeljno";

                case TestPeriodicity.biweekly:
                    return "Dvonedeljno";

                case TestPeriodicity.monthly:
                    return "Mesečno";

                case TestPeriodicity.yearly:
                    return "Godišnje";

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
