using Midi;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    public class OrpheeNoteMessage : IOrpheeNoteMessage
    {
        public int DeltaTime { get; set; }
        public byte MessageCode { get; set; }
        public int Channel { get; set; }
        public Note Note { get; set; }
        public int Velocity { get; set; }
    }
}
