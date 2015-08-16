using Newtonsoft.Json;

namespace Orphee.RestApiManagement.Interfaces
{
    public interface IUserData
    { 
        [JsonProperty(PropertyName = "token") ]
        string Token { get; set; }
        [JsonProperty(PropertyName = "user")]
        User User { get; set; }
    }
}
