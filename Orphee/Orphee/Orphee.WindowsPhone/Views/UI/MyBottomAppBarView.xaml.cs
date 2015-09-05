using Windows.UI.Xaml.Controls;
using Microsoft.Practices.Prism.Mvvm.Interfaces;

namespace Orphee.Views.UI
{
    public sealed partial class MyBottomAppBarView : UserControl
    {
        private INavigationService _navigationService;

        public MyBottomAppBarView()
        {
            this.InitializeComponent();
        }
    }
}
