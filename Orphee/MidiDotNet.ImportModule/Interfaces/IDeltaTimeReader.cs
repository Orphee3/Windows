using System.IO;

namespace MidiDotNet.ImportModule.Interfaces
{
    /// <summary>
    /// DeltaTimeReader interface.
    /// </summary>
    public interface IDeltaTimeReader
    {
        // Properties

        // Methods

        /// <summary>
        /// Function reading and converting the deltaTime from a byte array
        /// to an int.
        /// </summary>
        /// <param name="reader">Instance of the BinaryReader class read the noteMessage in the MIDI file</param>
        /// <returns>Returns the converted int value of the delta time</returns>
        int GetIntDeltaTime(BinaryReader reader);
    }
}
