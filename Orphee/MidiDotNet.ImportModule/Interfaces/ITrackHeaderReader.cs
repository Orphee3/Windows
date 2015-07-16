using System.IO;

namespace MidiDotNet.ImportModule.Interfaces
{
    public interface ITrackHeaderReader
    {
        // Properties
        uint TrackLength { get; }

        // Methods
        bool ReadTrackHeader(BinaryReader reader);
    }
}
