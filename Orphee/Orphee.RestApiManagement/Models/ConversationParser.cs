using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public void ParseConversationList(List<Conversation> conversationlist)
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
            conversation.Name = string.Empty;
            foreach (var user in conversation.UserList)
            {
                conversation.Name += user.Name;
                if (user != conversation.UserList.Last())
                    conversation.Name += ", ";
            }
        }
    }
}
