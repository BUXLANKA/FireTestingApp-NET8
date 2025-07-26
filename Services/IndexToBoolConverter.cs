using System.Globalization;
using System.Windows.Data;

///////////////////////////////////////////////////////////////////////////
//                              ШАБЛОННЫЙ КOД                            //
///////////////////////////////////////////////////////////////////////////

namespace FireTestingApp_net8.Services
{
    public class IndexToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string paramStr && int.TryParse(paramStr, out int paramInt))
                return (int?)value == paramInt;

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value && parameter?.ToString() is string s && int.TryParse(s, out int paramInt))
                return paramInt;

            return Binding.DoNothing;
        }
    }
}
