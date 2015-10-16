using System.IO;

namespace MidiDotNet.ImportModule.Interfaces
{
    /// <summary>
    /// ProgramChangeMessageReader interface.
    /// </summary>
    public interface IProgramChangeMessageReader
    {
        // Properties

        /// <summary>Value representing the instrument index of the instrument that is going to replace the actual one</summary>
        int InstrumentIndex { get; }
        /// <summary>Value representing current channel to which the programChangeMessage is applied</summary>
        int Channel { get; }

        // Methods

        /// <summary>
        /// Function reading the programChangeMessage of 
        /// the MIDI file
        /// </summary>
        /// <param name="reader">Instance of the BinaryReader class read the noteMessage in the MIDI file</param>
        /// <returns>Returns true if the message was read correctly and false if it wasn't</returns>
        bool ReadProgramChangeMessage(BinaryReader reader);
    }
}
