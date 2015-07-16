using System.IO;

namespace MidiDotNet.ImportModule.Interfaces
{
    public interface IFileHeaderReader
    {
        // Properties

        // Methods
        bool ReadFileHeader(BinaryReader reader);
    }
}
