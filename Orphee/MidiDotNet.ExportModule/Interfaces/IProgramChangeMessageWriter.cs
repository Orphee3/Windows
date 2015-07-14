using System.IO;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ExportModule.Interfaces
{
    public interface IProgramChangeMessageWriter
    {
        // Properties

        // Methods
        void WriteProgramChangeMessage(BinaryWriter writer, IOrpheeTrack orpheeTrack);
    }
}
