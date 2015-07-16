using System.IO;

namespace MidiDotNet.ImportModule.Interfaces
{
    public interface IEndOfTrackMessageReader
    {
        // Properties

        // Methods
        bool ReadEndOfTrackMessage(BinaryReader reader);
    }
}
