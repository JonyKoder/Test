using FontAwesome.WPF;
using System.Windows;
using System.Windows.Controls;

namespace Vodovoz.UI.Controls {
    public class IconButton : Button {
        public FontAwesomeIcon Icon {
            get => (FontAwesomeIcon)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(FontAwesomeIcon), typeof(IconButton), new PropertyMetadata(FontAwesomeIcon.Ambulance));

        public string Text {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(IconButton), new PropertyMetadata(string.Empty));
    }
}
