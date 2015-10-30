using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Orphee.RestApiManagement.Models.Interfaces
{
    /// <summary>
    /// Conversation interface
    /// </summary>
    public interface IConversation
    {
        /// <summary>Conversation id </summary>
        [JsonProperty(PropertyName = "_id")]
        string Id { get; set; }
        /// <summary>Name of the conversation </summary>
        [JsonProperty(PropertyName = "name")]
        string Name { get; set; }
        [JsonProperty(PropertyName = "private")]
        bool IsPrivate { get; set; }
        /// <summary>Preview of the last message reveived </summary>
        [JsonProperty(PropertyName = "lastMessage")]
        Message LastMessagePreview { get; set; }
        /// <summary>Conversation participants </summary>
        [JsonProperty(PropertyName = "people")]
        JArray Users { get; set; }
        [JsonProperty(PropertyName = "peopleTmp")]
        JArray TemporaryUsers { get; set; }
        /// <summary>Messages contained in the conversation </summary>
        [JsonProperty(PropertyName = "messages")]
        JArray MessageList { get; set; }
        /// <summary>Conversation participants </summary>
        List<User> UserList { get; set; }
        /// <summary>Messages contained in the conversation </summary>
        List<Message> Messages { get; set; }  
        /// <summary>Conversation picture source </summary>
        string ConversationPictureSource { get; set; }
    }
}
