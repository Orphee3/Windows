using System.Runtime.Serialization;
using Midi;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    /// <summary>
    /// Representation of a MIDI noteMessage
    /// </summary>
    [DataContract]
    public class OrpheeNoteMessage : IOrpheeNoteMessage
    {
        public OrpheeNoteMessage()
        {
            
        }
        /// <summary>Delta time of the current noteMessage </summary>
        [DataMember]
        public int DeltaTime { get; set; }
        /// <summary>Code of the current messageCode </summary>
        [DataMember]
        public byte MessageCode { get; set; }
        /// <summary>Channel related to the current noteMessage </summary>
        [DataMember]
        public int Channel { get; set; }
        /// <summary>Note related to the current noteMessage </summary>
        [DataMember]
        public Note Note { get; set; }
        /// <summary>Velocity related to the current noteMessage </summary>
        [DataMember]
        public int Velocity { get; set; }
    }
}
