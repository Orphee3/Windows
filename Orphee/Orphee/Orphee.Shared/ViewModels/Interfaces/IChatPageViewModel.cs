using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Orphee.Models;
using Orphee.RestApiManagement;

namespace Orphee.ViewModels.Interfaces
{
    public interface IChatPageViewModel
    {
        DelegateCommand BackCommand { get; }
        DelegateCommand SendCommand { get; }
        string Message { get; set; }
        string ConversationName { get; set; }
        ObservableCollection<Message> Conversation { get; }
    }
}
