using System.IO;

namespace MidiDotNet.ImportModule.Interfaces
{
    public interface ITimeSignatureMessageReader
    {
        // Properties
        uint Nominator { get; }
        uint Denominator { get; }
        uint ClocksPerBeat { get; }
        uint NumberOf32ThNotePerBeat { get; }

        // Methods
        bool ReadTimeSignatureMessage(BinaryReader reader);
    }
}
