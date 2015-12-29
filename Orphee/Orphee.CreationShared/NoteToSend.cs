using Midi;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    public class NoteToSend : INoteToSend
    {
        public int LineIndex { get; set; }
        public int ColumnIndex { get; set; }
        public Note Note { get; set; }
        public Channel Channel { get; set; }
        public int Octave { get; set; }
    }
}
