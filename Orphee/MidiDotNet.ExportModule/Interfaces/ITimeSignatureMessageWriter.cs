using System.IO;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ExportModule.Interfaces
{
    public interface ITimeSignatureMessageWriter
    {
        // Properties

        // Methods
        void WriteTimeSignatureMessage(BinaryWriter writer, IPlayerParameters playerParameters);
    }
}
