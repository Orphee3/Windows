using System.IO;
using MidiDotNet.ExportModule.Interfaces;

namespace MidiDotNet.ExportModule
{
    /// <summary>
    /// This class writes the endOfTrack message at the end of each
    /// track composing the MIDI file.
    /// </summary>
    public class EndOfTrackMessageWriter : IEndOfTrackMessageWriter
    {
        /// <summary>
        /// Function writting the endOfTrack message at the end of
        /// each track composing the MIDI file.
        /// </summary>
        /// <param name="writer">Instance of the BinaryWriter class writting the endOfTrack message in the MIDI file</param>
        public void WriteEndOfTrackMessage(BinaryWriter writer)
        {
            writer.Write((byte) 0x00);
            writer.Write((byte) 0xFF);
            writer.Write((byte) 0x2F);
            writer.Write((byte) 0x00);
        }
    }
}
