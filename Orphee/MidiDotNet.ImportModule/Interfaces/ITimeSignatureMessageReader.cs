using System.IO;

namespace MidiDotNet.ImportModule.Interfaces
{
    /// <summary>
    /// TimeSignatureMessageReader interface.
    /// </summary>
    public interface ITimeSignatureMessageReader
    {
        // Properties

        /// <summary>Value representing the nominator of the actual time signature</summary>
        uint Nominator { get; }
        /// <summary>Value representing the denominator of the actual time signature</summary>
        uint Denominator { get; }
        /// <summary>Value representing the number of clocks per beat of the actual time signature</summary>
        uint ClocksPerBeat { get; }
        /// <summary>Value representing the number of 32th note per click beat of the actual time signature</summary>
        uint NumberOf32ThNotePerBeat { get; }

        // Methods

        /// <summary>
        /// Function reading the TimeSignatureMessage of the MIDI file
        /// </summary>
        /// <param name="reader">Instance of the BinaryReader class read the noteMessage in the MIDI file</param>
        /// <returns>Returns true if the message was read correctly and false if it wasn't</returns>
        bool ReadTimeSignatureMessage(BinaryReader reader);
    }
}
