using Midi;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    /// <summary>
    /// Representation of a MIDI noteMessage
    /// </summary>
    public class OrpheeNoteMessage : IOrpheeNoteMessage
    {
        /// <summary>Delta time of the current noteMessage </summary>
        public int DeltaTime { get; set; }
        /// <summary>Code of the current messageCode </summary>
        public byte MessageCode { get; set; }
        /// <summary>Channel related to the current noteMessage </summary>
        public int Channel { get; set; }
        /// <summary>Note related to the current noteMessage </summary>
        public Note Note { get; set; }
        /// <summary>Velocity related to the current noteMessage </summary>
        public int Velocity { get; set; }
    }
}
