using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Orphee.RestApiManagement.Models.Interfaces
{
    /// <summary>
    /// User interface
    /// </summary>
    public interface IUser
    {
        /// <summary>User id</summary>
        [JsonProperty(PropertyName = "_id")]
        string Id { get; set; }
        /// <summary>User UserName </summary>
        [JsonProperty(PropertyName = "username")]
        string UserName { get; set; }
        /// <summary>User name </summary>
        [JsonProperty(PropertyName = "name")]
        string Name { get; set; }
        /// <summary>User comment list </summary>
        [JsonProperty(PropertyName = "comments")]
        JArray Comments { get; set; }
        /// <summary>User like list </summary>
        [JsonProperty(PropertyName = "likes")]
        JArray Likes { get; set; }
        /// <summary>User creation list </summary>
        [JsonProperty(PropertyName = "creations")]
        JArray Creations { get; set; }
        /// <summary>User friend list </summary>
        [JsonProperty(PropertyName = "friends")]
        JArray Friends { get; set; }
        /// <summary>User picture path </summary>
        [JsonProperty(PropertyName = "picture")]
        string Picture { get; set; }
        /// <summary>User creation date  </summary>
        [JsonProperty(PropertyName = "dateCreation")]
        DateTime DateCreaion { get; set; }
        /// <summary>True if the user ahs been selected for a new conversation. False otherwise </summary>
        bool IsChecked { get; set; }
        /// <summary>List of pending friend asking </summary>
        List<User> PendingFriendList { get; set; }
        /// <summary>List of unviewed messages </summary>
        List<Message> PendingMessageList { get; set; }
        /// <summary>List of unviewes comments </summary>
        List<Comment> PendingCommentList { get; set; }
        /// <summary> If this user is a friend if the logged user, the add button is hidden. It's visible otherwise </summary>
        Visibility AddButtonVisibility { get; set; }
        /// <summary>True if a comment notification was received. False otherwise </summary>
        bool HasReceivedCommentNotification { get; set; }
        /// <summary>True if a frien notification was received. False otherwise </summary>
        bool HasReceivedFriendNotification { get; set; }
        /// <summary>True if a message notification was received. False otherwise </summary>
        bool HasReceivedMessageNotification { get; set; }
        /// <summary>True if a friend validation notification was received. False otherwise </summary>
        bool HasReceivedFriendConfirmationNotification { get; set; }
        /// <summary>True if a picture was uploaded with success. False otherwise </summary>
        bool PictureHasBeenUplaodedWithSuccess { get; set; }
    }
}
