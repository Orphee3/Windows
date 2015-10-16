using System;
using System.IO;
using MidiDotNet.ImportModule.Interfaces;

namespace MidiDotNet.ImportModule
{
    /// <summary>
    /// Class containing all the needed functions in order to
    /// read the timeSignatureMessage from the MIDI file
    /// </summary>
    public class TimeSignatureMessageReader : ITimeSignatureMessageReader
    {
        private readonly byte _expectedDeltaTime;
        private readonly byte _expectedMetaCode;
        private readonly byte _expectedMessageCode;
        private readonly byte _expectedNumberOfBytes;
        /// <summary>Value representing the nominator of the actual time signature</summary>
        public uint Nominator { get; private set; }
        /// <summary>Value representing the denominator of the actual time signature</summary>
        public uint Denominator { get; private set; }
        /// <summary>Value representing the number of clocks per beat of the actual time signature</summary>
        public uint ClocksPerBeat { get; private set; }
        /// <summary>Value representing the number of 32th note per click beat of the actual time signature</summary>
        public uint NumberOf32ThNotePerBeat { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public TimeSignatureMessageReader()
        {
            this._expectedDeltaTime = 0;
            this._expectedMetaCode = 0xFF;
            this._expectedMessageCode = 0x58;
            this._expectedNumberOfBytes = 4;
        }

        /// <summary>
        /// Function reading the TimeSignatureMessage of the MIDI file
        /// </summary>
        /// <param name="reader">Instance of the BinaryReader class read the noteMessage in the MIDI file</param>
        /// <returns>Returns true if the message was read correctly and false if it wasn't</returns>
        public bool ReadTimeSignatureMessage(BinaryReader reader)
        {
            if (reader == null || reader.BaseStream.Length - reader.BaseStream.Position < 8)
                return false;

            var deltaTime = reader.ReadByte();
            var metaCode = reader.ReadByte();
            var messageCode = reader.ReadByte();
            var numberOfBytes = reader.ReadByte();
            this.Nominator = reader.ReadByte();
            this.Denominator = (uint) Math.Pow(2, reader.ReadByte());
            this.ClocksPerBeat = reader.ReadByte();
            this.NumberOf32ThNotePerBeat = reader.ReadByte();
            return IsInfoAsExpected(deltaTime, metaCode, messageCode, numberOfBytes);
        }

        private bool IsInfoAsExpected(byte deltaTime, byte metaCode, byte messageCode, byte numberOfBytes)
        {
            return deltaTime == this._expectedDeltaTime && metaCode == this._expectedMetaCode && messageCode == this._expectedMessageCode && numberOfBytes == this._expectedNumberOfBytes;
        }
    }
}
