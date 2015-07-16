using System.IO;

namespace MidiDotNet.ImportModule.Interfaces
{
    public interface IFileHeaderReader
    {
        // Properties
        ushort FileType { get; }
        ushort NumberOfTracks { get; }
        ushort DeltaTicksPerQuarterNote { get; }

        // Methods
        bool ReadFileHeader(BinaryReader reader);
    }
}
