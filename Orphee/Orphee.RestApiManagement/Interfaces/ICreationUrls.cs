using Newtonsoft.Json;

namespace Orphee.RestApiManagement.Interfaces
{
    public interface ICreationUrls
    {
        [JsonProperty(PropertyName = "urlGet")]
        string GetUrl { get; set; }
        [JsonProperty(PropertyName = "urlPut")]
        string PutUrl { get; set; }
    }
}
