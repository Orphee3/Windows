using System;
using Newtonsoft.Json;

namespace Orphee.RestApiManagement.Models.Interfaces
{
    /// <summary>
    /// New interface
    /// </summary>
    public interface INews
    {
        /// <summary>News id</summary>
        [JsonProperty(PropertyName = "_id")]
        string Id { get; set; }
        /// <summary>Creator of the news</summary>
        [JsonProperty(PropertyName = "userSource")]
        UserBase Creator { get; set; }
        /// <summary>Creation which the news comes from</summary>
        [JsonProperty(PropertyName = "media")]
        Creation Creation { get; set; }
        /// <summary>Type of the news</summary>
        [JsonProperty(PropertyName = "type")]
        string Type { get; set; }
        /// <summary>Creation date of the news</summary>
        [JsonProperty(PropertyName = "dateCreation")]
        DateTime DateCreation { get; set; }
        /// <summary>True if the news was viewed. False otherwise</summary>
        [JsonProperty(PropertyName = "viewed")]
        bool HasBeenViewed { get; set; }
        string Message { get; set; }
    }
}
