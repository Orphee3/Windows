using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Orphee.RestApiManagement.Models;

namespace Orphee.ViewModels.Interfaces
{
    /// <summary>
    /// ChatPageViewModel instance
    /// </summary>
    public interface IChatPageViewModel
    {
        /// <summary>Redirects to the previous page </summary>
        DelegateCommand BackCommand { get; }
        /// <summary>Sends a message to the other users participating at the conversation </summary>
        DelegateCommand SendCommand { get; }
        /// <summary>Message to be sent </summary>
        string Message { get; set; }
        /// <summary>Name of the conversation </summary>
        string ConversationName { get; set; }
        /// <summary>List of messages </summary>
        ObservableCollection<Message> Conversation { get; }

        /// <summary>
        /// Initialize the conversation with the given
        /// message list
        /// </summary>
        /// <param name="messages">Messages used to initialize the conversation</param>
        void InitConversation(List<Message> messages);
    }
}
