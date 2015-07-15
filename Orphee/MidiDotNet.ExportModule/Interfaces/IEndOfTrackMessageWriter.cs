using System.IO;

namespace MidiDotNet.ExportModule.Interfaces
{
    public interface IEndOfTrackMessageWriter
    {
        // Properties

        // Methods
        void WriteEndOfTrackMessage(BinaryWriter writer);
    }
}
