using System.IO;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ExportModule.Interfaces
{
    public interface IFileHeaderWriter
    {
        // Properties

        // Methods
        bool WriteFileHeader(BinaryWriter writer, IOrpheeFileParameters orpheeFileParameters);
    }
}
