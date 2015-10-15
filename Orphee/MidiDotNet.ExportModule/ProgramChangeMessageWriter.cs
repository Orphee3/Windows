using System.IO;
using Midi;
using MidiDotNet.ExportModule.Interfaces;

namespace MidiDotNet.ExportModule
{
    /// <summary>
    /// Class containing the needed functions to write programChange
    /// events in the MIDI file
    /// </summary>
    public class ProgramChangeMessageWriter : IProgramChangeMessageWriter
    {
        /// <summary>
        /// Function writting the programChange event
        /// in the MIDI file
        /// </summary>
        /// <param name="writer">Instance of the BinaryWriter class writting the noteMessages in the MIDI file</param>
        /// <param name="channel">Value representing the Channel related to the actual track for which the instrument is about to change</param>
        /// <param name="instrument">Instrument related to the actual track</param>
        public void WriteProgramChangeMessage(BinaryWriter writer, int channel, Instrument instrument)
        {
            writer.Write((byte) 0x00);
            writer.Write((byte) (0xC0 + (byte) channel));
            writer.Write((byte) instrument);
        }
    }
}
