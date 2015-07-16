using System.IO;

namespace MidiDotNet.ImportModule.Interfaces
{
    public interface IDeltaTimeReader
    {
        // Properties

        // Methods
        int GetIntDeltaTime(BinaryReader reader);
    }
}
