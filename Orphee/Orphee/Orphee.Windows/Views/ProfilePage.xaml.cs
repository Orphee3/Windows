using Windows.UI.Xaml.Input;
using Microsoft.Practices.Prism.Mvvm;

namespace Orphee.Views
{
    public sealed partial class ProfilePage : IView
    {
        public ProfilePage()
        {
            this.InitializeComponent();
      
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
