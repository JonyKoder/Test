using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Vodovoz.UI.Converters
{
    public class IsValueLessThanParameter : MarkupExtension, IValueConverter
    {
        private static IsValueLessThanParameter _converter = null;
        
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter is null)
                _converter = new IsValueLessThanParameter();
            return _converter;
        }
        public object Convert(object value, Type targetType, object parameter,CultureInfo culture)
        {
            double ItemSize = (double)value;
            double MaxAllowed = double.Parse(parameter as string);
            return ItemSize < MaxAllowed ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
            => (int)parameter > 400 ? true : false;
    }


}
