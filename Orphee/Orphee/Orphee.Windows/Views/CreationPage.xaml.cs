using Windows.UI.Xaml.Input;
using Microsoft.Practices.Prism.Mvvm;

namespace Orphee.Views
{
    public sealed partial class CreationPage : IView
    {
        public CreationPage()
        {
            this.InitializeComponent();
        }

        private void UIElement_OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void UIElement_OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void UIElement_OnManipulationInertiaStarting(object sender, ManipulationInertiaStartingRoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void UIElement_OnManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
