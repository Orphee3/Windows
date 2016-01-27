using System.Runtime.Serialization;
using Midi;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    [DataContract]
    public class NoteToSend : INoteToSend
    {
        [DataMember]
        public int LineIndex { get; set; }
        [DataMember]
        public int ColumnIndex { get; set; }
        [DataMember]
        public Note Note { get; set; }
        [DataMember]
        public Channel Channel { get; set; }
        [DataMember]
        public int Octave { get; set; }
    }
}
