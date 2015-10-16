using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    /// <summary>
    /// Representation of the MIDI file parameters
    /// </summary>
    public class OrpheeFileParameters : IOrpheeFileParameters
    {
        /// <summary>Number of tracks contained in the actual MIDI track </summary>
        public ushort OrpheeFileType { get; set; }
        /// <summary>MIDI file type </summary>
        public ushort NumberOfTracks { get; set; }
        /// <summary>Delta ticks per quarter note</summary>
        public ushort DeltaTicksPerQuarterNote { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public OrpheeFileParameters()
        {
            this.NumberOfTracks = 1;
            this.OrpheeFileType = 1;
            this.DeltaTicksPerQuarterNote = 60;
        }
    }
}
