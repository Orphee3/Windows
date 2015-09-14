using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Orphee.RestApiManagement.Models.Interfaces
{
    public interface IComment
    {
        [JsonProperty(PropertyName = "_id")]
        string Id { get; set; }
        [JsonProperty(PropertyName = "message")]
        string Message { get; set; }
        [JsonProperty(PropertyName = "creator")]
        string CreatorId { get; set; }
        [JsonProperty(PropertyName = "child")]
        JArray Child { get; set; }
    }
}
