using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Orphee.RestApiManagement.Interfaces
{
    public interface IUser
    {
        [JsonProperty(PropertyName = "_id")]
        string Id { get; set; }
        [JsonProperty(PropertyName = "username")]
        string UserName { get; set; }
        [JsonProperty(PropertyName = "name")]
        string Name { get; set; }
        [JsonProperty(PropertyName = "comments")]
        JArray Comments { get; set; }
        [JsonProperty(PropertyName = "likes")]
        JArray Likes { get; set; }
        [JsonProperty(PropertyName = "creations")]
        JArray Creations { get; set; }
        [JsonProperty(PropertyName = "friends")]
        JArray Friends { get; set; }
        [JsonProperty(PropertyName = "picture")]
        string Picture { get; set; }
        [JsonProperty(PropertyName = "dateCreation")]
        DateTime DateCreaion { get; set; }
        bool IsChecked { get; set; }
        List<User> PendingFriendList { get; set; }
        List<Message> PendingMessageList { get; set; } 
        bool HasReceivedFriendNotification { get; set; }
        bool HasReceivedMessageNotification { get; set; }
        bool HasReceivedFriendValidationNotification { get; set; }
        bool HasReceivedFriendConfirmationNotification { get; set; }
        bool PictureHasBeenUplaodedWithSuccess { get; set; }
    }
}
