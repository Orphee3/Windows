using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Orphee.RestApiManagement.Models.Interfaces;

namespace Orphee.RestApiManagement.Models
{
    public class Conversation : IConversation
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LastMessagePreview { get; set; }
        public JArray Users { get; set; }
        public JArray MessageList { get; set; }
        public DateTime LastMessageDate { get; set; }
        public List<User> UserList { get; set; }
        public List<Message> Messages { get; set; } 
        public string ConversationPictureSource { get; set; }

        public Conversation()
        {
            this.UserList = new List<User>();
            this.Messages = new List<Message>();
        }
    }
}
