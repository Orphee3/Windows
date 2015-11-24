using System;
using System.Collections.ObjectModel;
using System.Linq;
using Orphee.RestApiManagement.Models.Interfaces;

namespace Orphee.RestApiManagement.Models
{
    public class ConversationParser : IConversationParser
    {

        public void ParseConversationList(ObservableCollection<Conversation> conversationlist)
        {
            foreach (var conversation in conversationlist)
            {
                if (RestApiManagerBase.Instance.UserData.User.ConversationList.Count(c => c.Id == conversation.Id) == 0)
                {
                    var result = FillConversationUserList(conversation);
                }
            }
            RestApiManagerBase.Instance.UserData.User.ConversationList.Reverse();
        }

        private bool FillConversationUserList(Conversation conversation)
        {
            if (conversation.UserList != null && conversation.UserList.Count > 0)
            {
                conversation.UserList.Remove(conversation.UserList.FirstOrDefault(u => u.Id == RestApiManagerBase.Instance.UserData.User.Id));
                InitConversationName(conversation);
            }
            RestApiManagerBase.Instance.UserData.User.ConversationList.Add(conversation);
            return true;
        }

        private void InitConversationName(Conversation conversation)
        {
            if (conversation.UserList.Count == 0 || conversation.UserList == null)
                return;
            conversation.Name = String.Empty;
            foreach (var user in conversation.UserList)
            {
                conversation.Name += user.Name;
                if (user != conversation.UserList.Last())
                    conversation.Name += " ,";
            }
            conversation.Name = GetSubstringIfTooLong(conversation.Name);
            InitConversationPreviewLastMessage(conversation);
        }

        private void InitConversationPreviewLastMessage(Conversation conversation)
        {
            if (conversation.Messages == null || conversation.Messages.Count == 0)
                return;
            conversation.LastMessagePreview = conversation.Messages.Last();
            conversation.LastMessagePreview.ReceivedMessage = GetSubstringIfTooLong(conversation.LastMessagePreview.ReceivedMessage);
            conversation.LastMessageDateString = conversation.Messages.Last().Hour;
            conversation.ConversationPictureSource = conversation.UserList[0].Picture;
        }

        public string GetSubstringIfTooLong(string stringToCheck)
        {
            return stringToCheck.Length >= 30 ? stringToCheck.Substring(0, 30) + "..." : stringToCheck;
        }
    }
}
