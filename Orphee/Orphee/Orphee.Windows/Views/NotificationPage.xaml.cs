using Windows.UI.Xaml;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Orphee.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NotificationPage : IView
    {
        public NotificationPage()
        {
            this.InitializeComponent();
            if (!RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
                this.TextBlock.Visibility = Visibility.Visible;
        }
    }
}
