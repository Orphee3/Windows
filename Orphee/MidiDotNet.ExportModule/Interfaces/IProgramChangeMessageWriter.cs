using System.IO;
using Midi;

namespace MidiDotNet.ExportModule.Interfaces
{
    /// <summary>
    ///  ProgramChangeMessageWriter interface.
    /// </summary>
    public interface IProgramChangeMessageWriter
    {
        // Properties

        // Methods
        /// <summary>
        /// Function writting the programChange event
        /// in the MIDI file
        /// </summary>
        /// <param name="writer">Instance of the BinaryWriter class writting the noteMessages in the MIDI file</param>
        /// <param name="channel">Value representing the Channel related to the actual track for which the instrument is about to change</param>
        /// <param name="instrument">Instrument related to the actual track</param>
        void WriteProgramChangeMessage(BinaryWriter writer, int channel, Instrument instrument);
    }
}
