using System.IO;

namespace MidiDotNet.ImportModule.Interfaces
{
    /// <summary>
    /// FileHeaderReader interface.
    /// </summary>
    public interface IFileHeaderReader
    {
        // Properties

        /// <summary>Value representing the the actual file type of the MIDI file </summary>
        ushort FileType { get; }
        /// <summary>Value representing the number of tracks contained in the MIDI file </summary>
        ushort NumberOfTracks { get; }
        /// <summary>Value representing the delta ticks per quarter notes </summary>
        ushort DeltaTicksPerQuarterNote { get; }

        // Methods

        /// <summary>
        /// Function reading the MIDI file header
        /// </summary>
        /// <param name="reader">Instance of the BinaryReader class read the noteMessage in the MIDI file</param>
        /// <returns>Returns true if the message was read correctly and false if it wasn't</returns>
        bool ReadFileHeader(BinaryReader reader);
    }
}
