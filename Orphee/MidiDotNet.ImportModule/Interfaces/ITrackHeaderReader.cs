using System.IO;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ImportModule.Interfaces
{
    /// <summary>
    /// TrackHeaderReader interface.
    /// </summary>
    public interface ITrackHeaderReader
    {
        // Properties
    
        /// <summary>Instance of the TimeSignatureMessageReader class</summary>
        ITimeSignatureMessageReader TimeSignatureMessageReader { get; }
        /// <summary>Instance of the TempoMessageReader class</summary>
        ITempoMessageReader TempoMessageReader { get; }
        /// <summary>Instance of the ProgramChangerMessageReader class</summary>
        IProgramChangeMessageReader ProgramChangeMessageReader { get; }
        /// <summary>Instance of the PlayerParameter class</summary>
        IPlayerParameters PlayerParameters { get; }
        /// <summary>Value representing the track length of the actual track </summary>
        uint TrackLength { get; }

        // Methods

        /// <summary>
        /// Function reading the track header of each track
        /// found in the MIDI file
        /// </summary>
        /// <param name="reader">Instance of the BinaryReader class read the noteMessage in the MIDI file</param>
        /// <param name="trackPos">Value representing the track position in the MIDI file</param>
        /// <returns>Returns true if the message was read correctly and false if it wasn't</returns>
        bool ReadTrackHeader(BinaryReader reader, int trackPos);
    }
}
