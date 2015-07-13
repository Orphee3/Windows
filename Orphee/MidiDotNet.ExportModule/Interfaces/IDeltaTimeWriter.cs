using System.IO;

namespace MidiDotNet.ExportModule.Interfaces
{
    public interface IDeltaTimeWriter
    {
        // Properties

        // Methods
        void WriteDeltaTime(int deltaTime, BinaryWriter writer, ref int trackLength);
    }
}
