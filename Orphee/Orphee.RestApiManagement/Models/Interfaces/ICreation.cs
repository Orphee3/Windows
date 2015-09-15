using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Orphee.RestApiManagement.Models.Interfaces
{
    public interface ICreation
    {
        [JsonProperty(PropertyName = "_id")]
        string Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        string Name { get; set; }
        [JsonProperty(PropertyName = "isPrivate")]
        bool IsPrivate { get; set; }
        [JsonProperty(PropertyName = "comments")]
        JArray Comments { get; set; }
        [JsonProperty(PropertyName = "creator")]
        JArray Creator { get; set; }
        [JsonProperty(PropertyName = "url")]
        string GetUrl { get; set; }
        int NumberOfComment { get; set; }
        int NumberOfLike { get; set; }
        List<User> CreatorList { get; set; } 
        void UpdateNumberOfLikeAndCommentValue();
    }
}
