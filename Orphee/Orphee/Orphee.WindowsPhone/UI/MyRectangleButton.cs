using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace Orphee.UI
{
    public sealed class MyRectangleButton : ToggleButton
    {
        public static readonly DependencyProperty ButtonBackgroundColorProperty = DependencyProperty.RegisterAttached("ButtonBackgroundColor", typeof(SolidColorBrush), typeof(MyRectangleButton), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));
        public static readonly DependencyProperty RectangleVisibilityProperty = DependencyProperty.RegisterAttached("RectangleVisibility", typeof(double), typeof(MyRectangleButton), new PropertyMetadata(Visibility.Collapsed));
        public MyRectangleButton()
        {
            this.DefaultStyleKey = typeof(MyRectangleButton);
        }

        public SolidColorBrush ButtonBackgroundColor
        {
            get { return (SolidColorBrush)GetValue(ButtonBackgroundColorProperty); }

            set
            {
                if (this.RectangleVisibility > 0)
                    SetValue(ButtonBackgroundColorProperty, value);
            }
        }

        public double RectangleVisibility
        {
            get { return (double) GetValue(RectangleVisibilityProperty); }
            set
            {
                SetValue(RectangleVisibilityProperty, value);
            }
        }
    }
}
