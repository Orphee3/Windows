using System.IO;

namespace MidiDotNet.ExportModule.Interfaces
{
    /// <summary>
    ///  EndOfTrackMessageWriter interface.
    /// </summary>
    public interface IEndOfTrackMessageWriter
    {
        // Properties

        // Methods
        /// <summary>
        /// Function writting the endOfTrack message at the end of
        /// each track composing the MIDI file.
        /// </summary>
        /// <param name="writer">Instance of the BinaryWriter class writting the endOfTrack message in the MIDI file</param>
        void WriteEndOfTrackMessage(BinaryWriter writer);
    }
}
