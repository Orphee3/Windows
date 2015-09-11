using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.Commands;
using Newtonsoft.Json.Linq;
using Orphee.RestApiManagement;

namespace Orphee.ViewModels.Interfaces
{
    public interface IMessagesPageViewModel
    {
        ObservableCollection<Conversation> ConversationList { get; set; }
        DelegateCommand CreateNewConversationCommand { get; }
        DelegateCommand LoginButton { get; }
        string DisconnectedMessage { get; }
        Visibility ButtonsVisibility { get; }
        Visibility ListViewVisibility { get; }
    }
}
