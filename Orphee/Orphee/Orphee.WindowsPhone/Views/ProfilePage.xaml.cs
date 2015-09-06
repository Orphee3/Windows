using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement;

namespace Orphee.Views
{
    public sealed partial class ProfilePage : IView
    {
        public ProfilePage()
        {
            this.InitializeComponent();
      
        }

        private void ConnectionGridTapped(object sender, TappedRoutedEventArgs e)
        {
            App.MyNavigationService.Navigate("Login", null);
        }

        private void TextBlockTapped(object sender, TappedRoutedEventArgs e)
        {
            var textblock = sender as TextBlock;
            App.MyNavigationService.Navigate(textblock.Name, null);
        }

        public void Connect(int connectionId, object target)
        {
            throw new NotImplementedException();
        }
    }
}
