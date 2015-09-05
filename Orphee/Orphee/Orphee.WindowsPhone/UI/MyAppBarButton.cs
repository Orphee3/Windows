using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Orphee.UI
{
    public sealed class MyAppBarButton : AppBarButton
    {
        public static readonly DependencyProperty NotificationDotVisibilityProperty = DependencyProperty.RegisterAttached("NotificationDotVisibility", typeof(Visibility), typeof(MyAppBarButton), new PropertyMetadata(Visibility.Collapsed));

        public MyAppBarButton()
        {
            this.DefaultStyleKey = typeof(MyAppBarButton);
        }

        public Visibility NotificationDotVisibility
        {
            get { return (Visibility)GetValue(NotificationDotVisibilityProperty); }

            set
            {
                SetValue(NotificationDotVisibilityProperty, value);
            }
        }
    }
}
