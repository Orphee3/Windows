using Newtonsoft.Json;

namespace Orphee.CreationShared.Interfaces
{
    /// <summary>
    /// OrpheeFileParameters interface
    /// </summary>
    public interface IOrpheeFileParameters
    {
        // Properties

        /// <summary>Number of tracks contained in the actual MIDI track </summary>
        [JsonProperty(PropertyName = "NumberOfTracks")]
        ushort NumberOfTracks { get; set; }
        /// <summary>MIDI file type </summary>
        [JsonProperty(PropertyName = "OrpheeFileType")]
        ushort OrpheeFileType { get; set; }
        /// <summary>Delta ticks per quarter note</summary>
        [JsonProperty(PropertyName = "DeltaTicksPerQuarterNote")]
        ushort DeltaTicksPerQuarterNote { get; set; }
        // Methods
    }
}
