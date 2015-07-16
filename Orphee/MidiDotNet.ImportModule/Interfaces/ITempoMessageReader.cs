using System.IO;

namespace MidiDotNet.ImportModule.Interfaces
{
    public interface ITempoMessageReader
    {
        // Properties
        int Tempo { get; }

        // Methods
        bool ReadTempoMessage(BinaryReader reader);
    }
}
