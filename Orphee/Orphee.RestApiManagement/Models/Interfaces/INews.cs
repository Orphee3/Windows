using System;
using Newtonsoft.Json;

namespace Orphee.RestApiManagement.Models.Interfaces
{
    public interface INews
    {
        [JsonProperty(PropertyName = "_id")]
        string Id { get; set; }
        [JsonProperty(PropertyName = "userSource")]
        User Creator { get; set; }
        [JsonProperty(PropertyName = "media")]
        Creation Creation { get; set; }
        [JsonProperty(PropertyName = "type")]
        string NewsType { get; set; }
        [JsonProperty(PropertyName = "dateCreation")]
        DateTime DateCreation { get; set; }
        [JsonProperty(PropertyName = "viewed")]
        bool HasBeenViews { get; set; }
    }
}
