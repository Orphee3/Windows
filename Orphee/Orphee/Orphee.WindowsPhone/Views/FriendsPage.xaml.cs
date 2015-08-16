using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Mvvm;

namespace Orphee.Views
{
    public sealed partial class FriendsPage : IView
    {
        public FriendsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        public void Connect(int connectionId, object target)
        {
            throw new System.NotImplementedException();
        }
    }
}
