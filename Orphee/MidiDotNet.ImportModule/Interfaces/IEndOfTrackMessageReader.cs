using System.IO;

namespace MidiDotNet.ImportModule.Interfaces
{
    /// <summary>
    /// EndOfTrackMessageReader interface.
    /// </summary>
    public interface IEndOfTrackMessageReader
    {
        // Properties

        // Methods
        /// <summary>
        /// Function reading the endOfTrack messagage at the end
        /// of each track.
        /// </summary>
        /// <param name="reader">Instance of the BinaryReader class read the noteMessage in the MIDI filed</param>
        /// <returns>Returns true if the message was read correctly and false if it wasn't</returns>
        bool ReadEndOfTrackMessage(BinaryReader reader);
    }
}
