using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Orphee.RestApiManagement.Models.Interfaces
{
    public interface IConversation
    {
        [JsonProperty(PropertyName = "_id")]
        string Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        string Name { get; set; }
        string LastMessagePreview { get; set; }
        [JsonProperty(PropertyName = "people")]
        JArray Users { get; set; }
        [JsonProperty(PropertyName = "messages")]
        JArray MessageList { get; set; }
        [JsonProperty(PropertyName = "lastMessageDate")]
        DateTime LastMessageDate { get; set; }
        List<User> UserList { get; set; }
        List<Message> Messages { get; set; }  
        string ConversationPictureSource { get; set; }
    }
}
