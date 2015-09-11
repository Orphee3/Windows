using System.Collections.Generic;

namespace Orphee.RestApiManagement.Interfaces
{
    public interface IConversation
    {
        string Name { get; set; }
        string LastMessagePreview { get; set; }
        List<User> UserList { get; set; }
        List<Message> Messages { get; set; }  
    }
}
