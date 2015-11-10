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
        List<string> Likes { get; set; }
        /// <summary>User creation list </summary>
        [JsonProperty(PropertyName = "creations")]
        List<string> Creations { get; set; }
        /// <summary>User friend list </summary>
        [JsonProperty(PropertyName = "friends")]
        List<string> Friends { get; set; }
        /// <summary>User picture path </summary>
        [JsonProperty(PropertyName = "picture")]
        string Picture { get; set; }
        /// <summary>User creation date  </summary>
        [JsonProperty(PropertyName = "dateCreation")]
        DateTime DateCreaion { get; set; }
    }
}
