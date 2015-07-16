using System.IO;

namespace MidiDotNet.ImportModule.Interfaces
{
    public interface INoteMessageReader
    {
        // Properties
        int DeltaTime { get; }
        int NoteIndex { get; }
        int Velocity { get; }

        // Methods
        bool ReadNoteMessage(BinaryReader reader);
    }
}
