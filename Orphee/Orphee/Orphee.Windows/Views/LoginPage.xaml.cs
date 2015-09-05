using Windows.UI.Xaml.Input;
using Microsoft.Practices.Prism.Mvvm;


namespace Orphee.Views
{
    public sealed partial class LoginPage : IView
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            App.MyNavigationService.Navigate("Register", null);
        }
    }
}
