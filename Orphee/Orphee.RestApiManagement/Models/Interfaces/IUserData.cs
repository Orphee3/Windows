using Newtonsoft.Json;

namespace Orphee.RestApiManagement.Models.Interfaces
{
    /// <summary>
    /// UserData interface
    /// </summary>
    public interface IUserData
    { 
        /// <summary>User's connexion token </summary>
        [JsonProperty(PropertyName = "token") ]
        string Token { get; set; }
        /// <summary>User's representation containing all its related data</summary>
        [JsonProperty(PropertyName = "user")]
        User User { get; set; }
    }
}
