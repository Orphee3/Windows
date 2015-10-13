using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement.Models;

namespace Orphee.Views
{
    public sealed partial class MyCreationsPage : IView
    {
        public MyCreationsPage()
        {
            this.InitializeComponent();
        }

        private void Creation_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var creation = ((Grid) sender).DataContext as Creation;
            App.MyNavigationService.Navigate("CreationInfo", creation);
        }
    }
}
