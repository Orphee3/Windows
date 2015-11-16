using System;
using System.Collections.Generic;
using System.Numerics;
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
        [JsonProperty(PropertyName = "googleId")]
        string GoogleId { get; set; }
        [JsonProperty(PropertyName = "password")]
        string Password { get; set; }
        /// <summary>User UserName </summary>
        [JsonProperty(PropertyName = "username")]
        string UserName { get; set; }
        /// <summary>User name </summary>
        [JsonProperty(PropertyName = "name")]
        string Name { get; set; }
        [JsonProperty(PropertyName = "rooms")]
        JArray Rooms { get; set; }
        [JsonProperty(PropertyName = "news")]
        JArray News { get; set; }
        [JsonProperty(PropertyName = "flux")]
        JArray Flux { get; set; }
        [JsonProperty(PropertyName = "isAdmin")]
        bool IsAdmin { get; set; }
        /// <summary>User friend list </summary>
        [JsonProperty(PropertyName = "friends")]
        List<string> Friends { get; set; }
        /// <summary>User comment list </summary>
        [JsonProperty(PropertyName = "comments")]
        JArray Comments { get; set; }
        /// <summary>User like list </summary>
        [JsonProperty(PropertyName = "likes")]
        List<string> Likes { get; set; }
        [JsonProperty(PropertyName = "groups")]
        JArray Groups { get; set; }
        /// <summary>User creation list </summary>
        [JsonProperty(PropertyName = "creations")]
        List<string> Creations { get; set; }
        /// <summary>User picture path </summary>
        [JsonProperty(PropertyName = "picture")]
        string Picture { get; set; }
        /// <summary>User creation date  </summary>
        [JsonProperty(PropertyName = "dateCreation")]
        DateTime DateCreaion { get; set; }
    }
}
