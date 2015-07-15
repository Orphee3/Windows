using System.IO;

namespace MidiDotNet.ImportModule.Interfaces
{
    public interface IDeltaTimeRetriever
    {
        // Properties

        // Methods
        int GetIntDeltaTime(BinaryReader reader);
    }
}
