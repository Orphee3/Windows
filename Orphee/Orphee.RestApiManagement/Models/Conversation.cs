using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Orphee.RestApiManagement.Models.Interfaces;

namespace Orphee.RestApiManagement.Models
{
    /// <summary>
    /// Class containing all the conversation data
    /// </summary>
    public class Conversation : IConversation
    {
        /// <summary>Conversation id </summary>
        public string Id { get; set; }
        /// <summary>Name of the conversation </summary>
        public string Name { get; set; }
        /// <summary>Preview of the last message reveived </summary>
        public string LastMessagePreview { get; set; }
        /// <summary>Conversation participants </summary>
        public JArray Users { get; set; }
        /// <summary>Messages contained in the conversation </summary>
        public JArray MessageList { get; set; }
        /// <summary>Date of the last message </summary>
        public DateTime LastMessageDate { get; set; }
        /// <summary>Conversation participants </summary>
        public List<User> UserList { get; set; }
        /// <summary>Messages contained in the conversation </summary>
        public List<Message> Messages { get; set; }
        /// <summary>Conversation picture source </summary>
        public string ConversationPictureSource { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Conversation()
        {
            this.UserList = new List<User>();
            this.Messages = new List<Message>();
        }
    }
}
