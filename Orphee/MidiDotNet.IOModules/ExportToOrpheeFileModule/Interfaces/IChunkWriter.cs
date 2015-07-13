using System.IO;
using MidiDotNet.IOModules.Interfaces;

namespace MidiDotNet.IOModules.ExportToOrpheeFileModule.Interfaces
{
    public interface IChunkWriter
    {
        // Properties

        // Methods
        bool Write(BinaryWriter writer, IOrpheeFileParameters orpheeFileParameters);
    }
}
