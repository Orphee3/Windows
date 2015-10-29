using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Models.Interfaces;

namespace Orphee.RestApiManagement.Models
{
    public class ConversationParser : IConversationParser
    {
        private IGetter _getter;
        public ConversationParser(IGetter getter)
        {
            this._getter = getter;
        }

        public async void ParseConversationList(List<Conversation> conversationList)
        {
            foreach (var conversation in conversationList)
            {
                if (RestApiManagerBase.Instance.UserData.User.ConversationList.Count(c => c.Id == conversation.Id) == 0)
                {
                    var result = await FillConversationUserList(conversation);
                }
            }
            RestApiManagerBase.Instance.UserData.User.ConversationList.Reverse();
        }

        private async Task<bool> FillConversationUserList(Conversation conversation)
        {
            if (conversation.Users != null && conversation.Users.Count != 0)
            {
                var users = conversation.Users.Concat(conversation.TemporaryUsers).ToList();
                foreach (var user in users)
                {
                    var conversationUser = JsonConvert.DeserializeObject<User>(user.ToString());
                    if (conversationUser.Id != RestApiManagerBase.Instance.UserData.User.Id)
                        conversation.UserList.Add(conversationUser);
                }
                conversation.ConversationPictureSource = conversation.UserList.First().Picture;
                InitConversationName(conversation);
            }
            RestApiManagerBase.Instance.UserData.User.ConversationList.Add(conversation);
            return true;
        }

        private void InitConversationName(Conversation conversation)
        {
            if (conversation.UserList.Count == 1)
                conversation.Name = conversation.UserList[0].Name;
            else
            {
                foreach (var user in conversation.UserList)
                {
                    conversation.Name += user.Name;
                    if (user != conversation.UserList.Last())
                        conversation.Name += ", ";
                }
            }
        }
    }
}
