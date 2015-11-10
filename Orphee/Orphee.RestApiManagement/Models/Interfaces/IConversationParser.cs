using System.Collections.Generic;

namespace Orphee.RestApiManagement.Models.Interfaces
{
    public interface IConversationParser
    {
        void ParseConversationList(List<Conversation> conversationList);
    }
}
