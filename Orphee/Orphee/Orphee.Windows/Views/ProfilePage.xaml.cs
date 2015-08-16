using Windows.UI.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.ViewModels.Interfaces;

namespace Orphee.Views
{
    public sealed partial class ProfilePage : IView
    {
        public ProfilePage()
        {
            this.InitializeComponent();
        }
        private void Grid_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var grid = (Grid) sender;
            var stackPanel = (StackPanel) grid.Children[0];
            foreach (var child in stackPanel.Children)
                if (child is TextBlock)
                    ((IProfilePageViewModel)this.DataContext).PlaySelectedFile(((TextBlock)child).Text);
        }
    }
}
