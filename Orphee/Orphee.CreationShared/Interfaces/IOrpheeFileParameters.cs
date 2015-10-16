namespace Orphee.CreationShared.Interfaces
{
    /// <summary>
    /// OrpheeFileParameters interface
    /// </summary>
    public interface IOrpheeFileParameters
    {
        // Properties

        /// <summary>Number of tracks contained in the actual MIDI track </summary>
        ushort NumberOfTracks { get; set; }
        /// <summary>MIDI file type </summary>
        ushort OrpheeFileType { get; set; }
        /// <summary>Delta ticks per quarter note</summary>
        ushort DeltaTicksPerQuarterNote { get; set; }
        // Methods
    }
}
