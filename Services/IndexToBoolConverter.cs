using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FireTestingApp_net8.Services
{
    public class IndexToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //return (int?)value == int.Parse(parameter.ToString());

            if (parameter is string paramStr && int.TryParse(paramStr, out int paramInt))
                return (int?)value == paramInt;

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //return (bool)value ? int.Parse(parameter.ToString()) : Binding.DoNothing;

            if ((bool)value && parameter?.ToString() is string s && int.TryParse(s, out int paramInt))
                return paramInt;

            return Binding.DoNothing;
        }
    }
}
