using System.IO;

namespace MidiDotNet.ExportModule.Interfaces
{
    /// <summary>
    ///  DeltaTimeWriter interface.
    /// </summary>
    public interface IDeltaTimeWriter
    {
        // Properties

        // Methods
        /// <summary>
        /// Function writting the delta time of each MIDI message
        /// in the MIDI file.
        /// </summary>
        /// <param name="writer">Instance of the BinaryWriter class writting the noteMessage in the MIDI file</param>
        /// <param name="deltaTime">Delta time value to write</param>
        void WriteDeltaTime(BinaryWriter writer, int deltaTime);
    }
}
