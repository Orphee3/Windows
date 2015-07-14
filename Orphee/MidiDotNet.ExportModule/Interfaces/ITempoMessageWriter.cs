using System.IO;

namespace MidiDotNet.ExportModule.Interfaces
{
    public interface ITempoMessageWriter
    {
        // Properties 

        // Methods
        void WriteTempoMessage(BinaryWriter writer, uint tempo);
    }
}
