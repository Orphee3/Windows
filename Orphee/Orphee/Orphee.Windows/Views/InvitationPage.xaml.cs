using Windows.UI.Xaml;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement.Models;

namespace Orphee.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InvitationPage : IView
    {
        public InvitationPage()
        {
            this.InitializeComponent();
            if (!RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
                this.TextBlock.Visibility = Visibility.Visible;
        }
    }
}
