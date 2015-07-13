using System.IO;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ExportModule.Interfaces
{
    public interface ITrackHeaderWriter
    {
        // Properties

        // Methods
        bool WriteTrackHeader(BinaryWriter writer, IPlayerParameters playerParameters, int trackPos);
    }
}
