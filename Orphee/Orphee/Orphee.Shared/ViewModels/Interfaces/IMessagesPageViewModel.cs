using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.Commands;
using Newtonsoft.Json.Linq;

namespace Orphee.ViewModels.Interfaces
{
    public interface IMessagesPageViewModel
    {
        ObservableCollection<JToken> ConversationList { get; set; } 
        DelegateCommand LoginButton { get; }
        DelegateCommand AddCommand { get; }
        string DisconnectedMessage { get; }
        Visibility ButtonsVisibility { get; }
        Visibility ListViewVisibility { get; }
    }
}
