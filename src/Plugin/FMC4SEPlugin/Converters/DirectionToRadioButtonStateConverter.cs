using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace MDD4All.FMC4SE.Plugin.Converters
{
    public class DirectionToRadioButtonStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = false;

            string directionValue = (string)value;

            string parameterValue = (string)parameter;
            
            if(directionValue == parameterValue)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter;
        }
    }
}
