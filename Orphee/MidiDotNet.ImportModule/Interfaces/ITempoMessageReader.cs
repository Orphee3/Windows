using System.IO;

namespace MidiDotNet.ImportModule.Interfaces
{
    public interface ITempoMessageReader
    {
        // Properties
        uint Tempo { get; }

        // Methods
        bool ReadTempoMessage(BinaryReader reader);
    }
}
