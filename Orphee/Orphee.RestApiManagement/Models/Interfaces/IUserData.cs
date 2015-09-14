using Newtonsoft.Json;

namespace Orphee.RestApiManagement.Models.Interfaces
{
    public interface IUserData
    { 
        [JsonProperty(PropertyName = "token") ]
        string Token { get; set; }
        [JsonProperty(PropertyName = "user")]
        User User { get; set; }
    }
}
