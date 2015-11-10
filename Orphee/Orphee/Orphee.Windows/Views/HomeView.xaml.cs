using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Mvvm;

namespace Orphee.Views
{
    public sealed partial class HomePage : IView
    {
        public HomePage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }
    }
}
