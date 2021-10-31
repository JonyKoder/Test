using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using Vodovoz.DAL.Model;
using Vodovoz.UI.Manager;

namespace Vodovoz.UI.Converters
{
    public class NavigationConverter : MarkupExtension, IValueConverter
    {
        private static NavigationConverter _converter = null;
       
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter is null)
                _converter = new NavigationConverter();
            return _converter;
        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            NavigationModel navigateTo = (NavigationModel)value;
            NavigationService navigation = NavigationService.GetInstance;
            if (navigateTo is null)
                return null;
            return navigation.NavigateToModel(navigateTo);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => null;
    }


}
