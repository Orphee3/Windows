using System.IO;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ExportModule.Interfaces
{
    /// <summary>
    ///  TrackHeaderWriter interface.
    /// </summary>
    public interface ITrackHeaderWriter
    {
        // Properties


        // Methods
        /// <summary>
        /// Function writting the track header of every track
        /// </summary>
        /// <param name="writer">Instance of the BinaryWriter class writting the noteMessages in the MIDI file</param>
        /// <param name="playerParameters">Instance of the PlayerParameters containing all the data needed to create the timeSignatureMessage to be written</param>
        /// <param name="trackLength">Value representing the length of the processed track</param>
        bool WriteTrackHeader(BinaryWriter writer, IPlayerParameters playerParameters, uint trackLength);
    }
}
