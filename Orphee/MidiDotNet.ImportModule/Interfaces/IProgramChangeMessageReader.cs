using System.IO;

namespace MidiDotNet.ImportModule.Interfaces
{
    public interface IProgramChangeMessageReader
    {
        // Properties
        int InstrumentIndex { get; }
        int Channel { get; }

        // Methods
        bool ReadProgramChangeMessage(BinaryReader reader);
    }
}
