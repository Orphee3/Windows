using System.IO;
using MidiDotNet.ImportModule.Interfaces;

namespace MidiDotNet.ImportModule
{
    /// <summary>
    /// Class containing ell the functions needed in order to
    /// read the programChangeMessage of the MIDI file
    /// </summary>
    public class ProgramChangeMessageReader : IProgramChangeMessageReader
    {
        private readonly byte _expectedDeltaTime;
        private readonly byte _expectedMessageCode;
        /// <summary>Value representing the instrument index of the instrument that is going to replace the actual one</summary>
        public int InstrumentIndex { get; private set; }
        /// <summary>Value representing current channel to which the programChangeMessage is applied</summary>
        public int Channel { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ProgramChangeMessageReader()
        {
            this._expectedDeltaTime = 0;
            this._expectedMessageCode = 0xC0;
        }

        /// <summary>
        /// Function reading the programChangeMessage of 
        /// the MIDI file
        /// </summary>
        /// <param name="reader">Instance of the BinaryReader class read the noteMessage in the MIDI file</param>
        /// <returns>Returns true if the message was read correctly and false if it wasn't</returns>
        public bool ReadProgramChangeMessage(BinaryReader reader)
        {
            if (reader == null || reader.BaseStream.Length - reader.BaseStream.Position < 3)
                return false;
            var deltaTime = reader.ReadByte();
            var messageCode = reader.ReadByte();
            this.InstrumentIndex = reader.ReadByte();
            return IsInfoAsExpected(deltaTime, messageCode);
        }

        private bool IsInfoAsExpected(byte deltaTime, byte messageCode)
        {
            if (deltaTime != this._expectedDeltaTime || this._expectedMessageCode != (messageCode & 0xC0))
                return false;
            this.Channel = messageCode ^ 0xC0;
            return true;
        }
    }
}
