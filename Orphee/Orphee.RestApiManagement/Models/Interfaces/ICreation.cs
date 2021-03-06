﻿using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Orphee.RestApiManagement.Models.Interfaces
{
    /// <summary>
    /// Creation interface
    /// </summary>
    public interface ICreation
    {
        /// <summary>Creation id </summary>
        [JsonProperty(PropertyName = "_id")]
        string Id { get; set; }
        /// <summary>Creation name</summary>
        [JsonProperty(PropertyName = "name")]
        string Name { get; set; }
        /// <summary>True if the creation is private. False otherwise </summary>
        [JsonProperty(PropertyName = "isPrivate")]
        bool IsPrivate { get; set; }
        /// <summary>Comments related to the creation </summary>
        [JsonProperty(PropertyName = "comments")]
        JArray Comments { get; set; }
        /// <summary>Creation creator </summary>
        [JsonProperty(PropertyName = "creator")]
        List<UserBase> CreatorList { get; set; }
        /// <summary>AWS S3 creation get url</summary>
        [JsonProperty(PropertyName = "url")]
        string GetUrl { get; set; }
        /// <summary>Number of comments related to the creation</summary>
        [JsonProperty(PropertyName = "nbComments")]
        int NumberOfComment { get; set; }
        /// <summary>Number of likes related to the creation </summary>
        [JsonProperty(PropertyName = "nbLikes")]
        int NumberOfLike { get; set; }
        //[JsonProperty(PropertyName = "picture")]
        string Picture { get; set; }
    }
}
