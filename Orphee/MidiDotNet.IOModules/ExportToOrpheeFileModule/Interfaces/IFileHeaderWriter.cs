using System.IO;
using MidiDotNet.IOModules.Interfaces;

namespace MidiDotNet.IOModules.ExportToOrpheeFileModule.Interfaces
{
    public interface IFileHeaderWriter : IChunkWriter
    {
        // Properties

        // Methods
        new bool Write(BinaryWriter writer, IOrpheeFileParameters orpheeFileParameters);
    }
}
