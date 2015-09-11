using System;
using Newtonsoft.Json;

namespace Orphee.RestApiManagement.Interfaces
{
    public interface IMessage
    {
        [JsonProperty(PropertyName = "message")]
        string ReceivedMessage { get; set; }
        [JsonProperty(PropertyName = "creator")]
        User User { get; set; }
        [JsonProperty(PropertyName = "dateCreation")]
        DateTime Date { get; set; }
    }
}
