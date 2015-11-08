using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement.Models;

namespace Orphee.Views
{
    public sealed partial class HomePage : IView
    {
        public HomePage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            if (!RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
                this.TextBlock.Visibility = Visibility.Visible;
        }
    }
}
