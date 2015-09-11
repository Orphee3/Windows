using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Orphee.Models;

namespace Orphee.ViewModels.Interfaces
{
    public interface IChatPageViewModel
    {
        DelegateCommand BackCommand { get; }
        DelegateCommand SendCommand { get; }
        string Message { get; set; }
        string ConversationName { get; set; }
        ObservableCollection<MyDictionary> Conversation { get; }
    }
}
