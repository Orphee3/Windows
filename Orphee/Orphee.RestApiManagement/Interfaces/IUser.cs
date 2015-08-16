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
        Dictionary<string, ICreationUrls> CreationGetPutKeyList { get; set; }
    }
}
