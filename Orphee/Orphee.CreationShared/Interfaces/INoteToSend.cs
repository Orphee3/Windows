using Midi;

namespace Orphee.CreationShared.Interfaces
{
    public interface INoteToSend
    {
        int LineIndex { get; set; }
        int ColumnIndex { get; set; }
        Note Note { get; set; }
        Channel Channel { get; set; }
        int Octave { get; set; }
    }
}
