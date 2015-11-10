using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Orphee.RestApiManagement.Models.Interfaces
{
    /// <summary>
    /// Comment interface
    /// </summary>
    public interface IComment
    {
        /// <summary>Comment id </summary>
        [JsonProperty(PropertyName = "_id")]
        string Id { get; set; }
        /// <summary>Comment </summary>
        [JsonProperty(PropertyName = "message")]
        string Message { get; set; }
        /// <summary>Comment creator </summary>
        [JsonProperty(PropertyName = "creator")]
        UserBase Creator { get; set; }
        /// <summary>Comment repsonses</summary>
        [JsonProperty(PropertyName = "child")]
        JArray Child { get; set; }
        /// <summary>Creation id related to the comment </summary>
        [JsonProperty(PropertyName = "creation")]
        string CreationId { get; set; }
    }
}
