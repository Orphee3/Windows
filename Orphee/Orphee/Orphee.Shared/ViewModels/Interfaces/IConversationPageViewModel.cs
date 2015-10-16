using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.Commands;
using Orphee.RestApiManagement.Models;

namespace Orphee.ViewModels.Interfaces
{
    /// <summary>
    /// ConversationPage interface
    /// </summary>
    public interface IConversationPageViewModel
    {
        /// <summary>List of conversation that the user has </summary>
        ObservableCollection<Conversation> ConversationList { get; set; }
        /// <summary>Redirects to the FriendPage so the user can select which friend to include in the new conversation </summary>
        DelegateCommand CreateNewConversationCommand { get; }
        /// <summary>Redirect to the LoginPage </summary>
        DelegateCommand LoginButton { get; }
        /// <summary>Visible if the user is disconnected. Hidden otherwise </summary>
        Visibility ButtonsVisibility { get; set; }
        /// <summary>Visible if the user is connected. Hidder otherwise </summary>
        Visibility ListViewVisibility { get; set; }

        /// <summary>
        /// Creates a new conversation
        /// </summary>
        /// <param name="conversation">Contains the data related to the conversation to be created</param>
        void CreateNewConversation(Conversation conversation);
    }
}
