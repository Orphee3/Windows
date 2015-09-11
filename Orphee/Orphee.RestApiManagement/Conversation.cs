using System.Collections.Generic;
using Orphee.RestApiManagement.Interfaces;

namespace Orphee.RestApiManagement
{
    public class Conversation : IConversation
    {
        public string Name { get; set; }
        public string LastMessagePreview { get; set; }
        public List<User> UserList { get; set; }
        public List<Message> Messages { get; set; } 

        public Conversation()
        {
            this.UserList = new List<User>();
            this.Messages = new List<Message>();
        }
    }
}
