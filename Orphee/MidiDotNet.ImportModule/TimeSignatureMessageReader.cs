using System;
using System.IO;
using MidiDotNet.ImportModule.Interfaces;

namespace MidiDotNet.ImportModule
{
    public class TimeSignatureMessageReader : ITimeSignatureMessageReader
    {
        private readonly byte _expectedDeltaTime;
        private readonly byte _expectedMetaCode;
        private readonly byte _expectedMessageCode;
        private readonly byte _expectedNumberOfBytes;
        public uint Nominator { get; private set; }
        public uint Denominator { get; private set; }
        public uint ClocksPerBeat { get; private set; }
        public uint NumberOf32ThNotePerBeat { get; private set; }

        public TimeSignatureMessageReader()
        {
            this._expectedDeltaTime = 0;
            this._expectedMetaCode = 0xFF;
            this._expectedMessageCode = 0x58;
            this._expectedNumberOfBytes = 4;
        }

        private bool IsInfoAsExpected(byte deltaTime, byte metaCode, byte messageCode, byte numberOfBytes)
        {
            return deltaTime == this._expectedDeltaTime && metaCode == this._expectedMetaCode && messageCode == this._expectedMessageCode && numberOfBytes == this._expectedNumberOfBytes;
        }

        public bool ReadTimeSignatureMessage(BinaryReader reader)
        {
            if (reader == null || reader.BaseStream.Length - reader.BaseStream.Position < 8)
                return false;

            var deltaTime = reader.ReadByte();
            var metaCode = reader.ReadByte();
            var messageCode = reader.ReadByte();
            var numberOfBytes = reader.ReadByte();
            this.Nominator = reader.ReadByte();
            this.Denominator = (uint) Math.Pow(2, (int)reader.ReadByte());
            this.ClocksPerBeat = reader.ReadByte();
            this.NumberOf32ThNotePerBeat = reader.ReadByte();
            return IsInfoAsExpected(deltaTime, metaCode, messageCode, numberOfBytes);
        }
    }
}
