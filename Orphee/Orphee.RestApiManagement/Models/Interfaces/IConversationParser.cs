using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orphee.RestApiManagement.Models.Interfaces
{
    public interface IConversationParser
    {
        void ParseConversationList(List<Conversation> conversationList);
    }
}
