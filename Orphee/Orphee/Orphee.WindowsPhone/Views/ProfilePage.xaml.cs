using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Mvvm;

namespace Orphee.Views
{
    public sealed partial class ProfilePage : IView
    {
        public ProfilePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.MyBottomAppBar.Unload();
        }
    }
}
