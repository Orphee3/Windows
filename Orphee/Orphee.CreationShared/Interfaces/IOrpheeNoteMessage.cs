using Midi;
using Newtonsoft.Json;

namespace Orphee.CreationShared.Interfaces
{
    /// <summary>
    /// OrpheeNoteMessage interface
    /// </summary>
    public interface IOrpheeNoteMessage
    {
        // Properties
        /// <summary>Delta time of the current noteMessage </summary>
        [JsonProperty(PropertyName = "DeltaTime")]
        int DeltaTime { get; set; }
        /// <summary>Code of the current messageCode </summary>
        [JsonProperty(PropertyName = "MessageCode")]
        byte MessageCode { get; set; }
        /// <summary>Channel related to the current noteMessage </summary>
        [JsonProperty(PropertyName = "Channel")]
        int Channel { get; set; }
        /// <summary>Note related to the current noteMessage </summary>
        [JsonProperty(PropertyName = "Note")]
        Note Note { get; set; }
        /// <summary>Velocity related to the current noteMessage </summary>
        [JsonProperty(PropertyName = "Velocity")]
        int Velocity { get; set; }

        // Methods

    }
}
