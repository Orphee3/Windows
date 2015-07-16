using System.IO;

namespace MidiDotNet.ImportModule.Interfaces
{
    public interface ITimeSignatureMessageReader
    {
        // Properties
        int Nominator { get; }
        int Denominator { get; }
        int ClocksPerBeat { get; }
        int NumberOf32ThNotePerBeat { get; }

        // Methods
        bool ReadTimeSignatureMessage(BinaryReader reader);
    }
}
