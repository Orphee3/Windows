using System.IO;

namespace MidiDotNet.ImportModule.Interfaces
{
    /// <summary>
    /// TempoMessageReader interface.
    /// </summary>
    public interface ITempoMessageReader
    {
        // Properties

        /// <summary>Value representing the tempo</summary>
        uint Tempo { get; }

        // Methods

        /// <summary>
        /// Function reading the tempo message of the
        /// MIDI file
        /// </summary>
        /// <param name="reader">Instance of the BinaryReader class read the noteMessage in the MIDI file</param>
        /// <returns>Returns true if the message was read correctly and false if it wasn't</returns>
        bool ReadTempoMessage(BinaryReader reader);
    }
}
