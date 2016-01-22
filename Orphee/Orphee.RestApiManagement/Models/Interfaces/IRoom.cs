using System.Collections.Generic;
using Newtonsoft.Json;

namespace Orphee.RestApiManagement.Models.Interfaces
{
    public interface IRoom
    {
        [JsonProperty(PropertyName = "people")]
        List<string> People { get; set; }
        [JsonProperty(PropertyName = "id")]
        string Id { get; set; }
        [JsonProperty(PropertyName = "type")]
        string Type { get; set; }
        [JsonProperty(PropertyName = "host")]
        string Host { get; set; }
    }
}
