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

        private void UIElement_OnHolding(object sender, HoldingRoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
