using System.IO;
using MidiDotNet.ImportModule.Interfaces;

namespace MidiDotNet.ImportModule
{
    public class ProgramChangeMessageReader : IProgramChangeMessageReader
    {
        private readonly byte _expectedDeltaTime;
        private readonly byte _expectedMessageCode;
        public int InstrumentIndex { get; private set; }
        public int Channel { get; private set; }

        public ProgramChangeMessageReader()
        {
            this._expectedDeltaTime = 0;
            this._expectedMessageCode = 0xC0;
        }

        private bool IsInfoAsExpected(byte deltaTime, byte messageCode)
        {
            if (deltaTime != this._expectedDeltaTime || this._expectedMessageCode != (messageCode & 0xC0))
                return false;
            this.Channel = messageCode ^ 0xC0;
            return true;
        }

        public bool ReadProgramChangeMessage(BinaryReader reader)
        {
            if (reader == null || reader.BaseStream.Length - reader.BaseStream.Position < 3)
                return false;
            var deltaTime = reader.ReadByte();
            var messageCode = reader.ReadByte();
            this.InstrumentIndex = reader.ReadByte();
            return IsInfoAsExpected(deltaTime, messageCode);
        }
    }
}
