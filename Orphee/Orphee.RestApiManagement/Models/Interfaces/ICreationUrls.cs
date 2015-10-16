using Newtonsoft.Json;

namespace Orphee.RestApiManagement.Models.Interfaces
{
    /// <summary>
    /// CreationUrls interface
    /// </summary>
    public interface ICreationUrls
    {
        /// <summary>Get url of the related file </summary>
        [JsonProperty(PropertyName = "urlGet")]
        string GetUrl { get; set; }
        /// <summary>Put url of the related file </summary>
        [JsonProperty(PropertyName = "urlPut")]
        string PutUrl { get; set; }
    }
}
