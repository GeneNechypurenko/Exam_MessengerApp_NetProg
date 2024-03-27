using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MessengerApp_Client.Helpers
{
    public class MessageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value?.ToString();
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value;
    }
}