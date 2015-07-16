using System;
using System.IO;
using MidiDotNet.ImportModule.Interfaces;

namespace MidiDotNet.ImportModule
{
    public class TempoMessageReader : ITempoMessageReader
    {
        private readonly byte _expectedDeltaTime;
        private readonly byte _expectedMetaCode;
        private readonly byte _expectedMessageCode;
        private readonly byte _expectedNumberOfBytes;
        public int Tempo { get; private set; }

        public TempoMessageReader()
        {
            this._expectedDeltaTime = 0;
            this._expectedMetaCode = 0xFF;
            this._expectedMessageCode = 0x51;
            this._expectedNumberOfBytes = 3;
        }

        private bool IsInfoAsExpected(byte deltaTime, byte metaCode, byte messageCode)
        {
            return deltaTime == this._expectedDeltaTime && metaCode == _expectedMetaCode && messageCode == this._expectedMessageCode && _expectedNumberOfBytes == this._expectedNumberOfBytes;
        }

        private bool RetriveTempo(BinaryReader reader)
        {
            var tempoBytes = new byte[4];

            tempoBytes[0] = 0;
            for (var iterator = 1; iterator < 4; iterator++)
                tempoBytes[iterator] = reader.ReadByte();
            Array.Reverse(tempoBytes);
            this.Tempo = 60000000 / BitConverter.ToInt32(tempoBytes, 0);
            return this.Tempo >= 40 && this.Tempo <= 400;
        }

        public bool ReadTempoMessage(BinaryReader reader)
        {
            if (reader == null || reader.BaseStream.Length - reader.BaseStream.Position < 7)
                return false;
            var deltaTime = reader.ReadByte();
            var metaCode = reader.ReadByte();
            var messageCode = reader.ReadByte();
            return RetriveTempo(reader) && IsInfoAsExpected(deltaTime, metaCode, messageCode);
        }
    }
}
