using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orphee.RestApiManagement.Models.Interfaces;

namespace Orphee.RestApiManagement.Models
{
    public class ConversationParser : IConversationParser
    {
        public void ParseConversationList(List<Conversation> conversationList)
        {
            foreach (var conversation in conversationList)
            {
                if (RestApiManagerBase.Instance.UserData.User.ConversationList.Count(c => c.Id == conversation.Id) == 0)
                    FillConversationUserList(conversation);
            }
            RestApiManagerBase.Instance.UserData.User.ConversationList.Reverse();
        }

        private void FillConversationUserList(Conversation conversation)
        {
            foreach (var user in conversation.Users)
                if (user.ToString() != RestApiManagerBase.Instance.UserData.User.Id)
                {
                    conversation.ConversationPictureSource = RestApiManagerBase.Instance.UserData.User.FriendList.FirstOrDefault(uf => uf.Id == user.ToString()).Picture;
                    conversation.UserList.Add(RestApiManagerBase.Instance.UserData.User.FriendList.FirstOrDefault(uf => uf.Id == user.ToString()));
                }
            InitConversationName(conversation);
            RestApiManagerBase.Instance.UserData.User.ConversationList.Add(conversation);
        }

        private void InitConversationName(Conversation conversation)
        {
            if (conversation.UserList.Count == 1)
                conversation.Name = conversation.UserList[0].Name;
        }
    }
}
