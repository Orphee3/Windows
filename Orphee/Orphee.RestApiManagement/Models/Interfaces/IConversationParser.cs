using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Orphee.RestApiManagement.Models.Interfaces
{
    public interface IConversationParser
    {
        void ParseConversationList(ObservableCollection<Conversation> conversationList);
        string GetSubstringIfTooLong(string stringToCheck);
    }
}
