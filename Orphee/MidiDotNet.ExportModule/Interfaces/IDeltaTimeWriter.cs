using System.IO;

namespace MidiDotNet.ExportModule.Interfaces
{
    public interface IDeltaTimeWriter
    {
        // Properties

        // Methods
        void WriteDeltaTime(BinaryWriter writer, int deltaTime);
    }
}
