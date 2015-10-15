using System.IO;

namespace MidiDotNet.ExportModule.Interfaces
{
    /// <summary>
    ///  TempoMessageWriter interface.
    /// </summary>
    public interface ITempoMessageWriter
    {
        // Properties 

        // Methods
        /// <summary>
        /// Function writting the tempoMessage
        /// in the MIDI file
        /// </summary>
        /// <param name="writer">Instance of the BinaryWriter class writting the noteMessages in the MIDI file</param>
        /// <param name="tempo">Value representing the tempo value to be written</param>
        void WriteTempoMessage(BinaryWriter writer, uint tempo);
    }
}
