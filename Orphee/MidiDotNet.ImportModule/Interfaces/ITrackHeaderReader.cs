using System.IO;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ImportModule.Interfaces
{
    public interface ITrackHeaderReader
    {
        // Properties
        ITimeSignatureMessageReader TimeSignatureMessageReader { get; }
        ITempoMessageReader TempoMessageReader { get; }
        IProgramChangeMessageReader ProgramChangeMessageReader { get; }
        IPlayerParameters PlayerParameters { get; }
        uint TrackLength { get; }

        // Methods
        bool ReadTrackHeader(BinaryReader reader, int trackPos);
    }
}
