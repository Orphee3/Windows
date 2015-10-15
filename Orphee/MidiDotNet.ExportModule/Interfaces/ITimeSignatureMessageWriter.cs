using System.IO;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ExportModule.Interfaces
{
    /// <summary>
    ///  TimeSignatureMessageWriter interface.
    /// </summary>
    public interface ITimeSignatureMessageWriter
    {
        // Properties

        // Methods
        /// <summary>
        /// Function writting the timeSignatureMessage
        /// in the MIDI file
        /// </summary>
        /// <param name="writer">Instance of the BinaryWriter class writting the noteMessages in the MIDI file</param>
        /// <param name="playerParameters">Instance of the PlayerParameters containing all the data needed to create the timeSignatureMessage to be written</param>
        void WriteTimeSignatureMessage(BinaryWriter writer, IPlayerParameters playerParameters);
    }
}
