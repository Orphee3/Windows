using System.IO;
using MidiDotNet.ImportModule.Interfaces;

namespace MidiDotNet.ImportModule
{
    public class EndOfTrackMessageReader : IEndOfTrackMessageReader
    {
        private readonly byte _expectedDeltaTime;
        private readonly byte _expectedMetaEventCode;
        private readonly byte _expectedMessageCode;
        private readonly byte _expectedData;

        public EndOfTrackMessageReader()
        {
            this._expectedDeltaTime = 0;
            this._expectedMetaEventCode = 0xFF;
            this._expectedMessageCode = 0x2F;
            this._expectedData = 0;
        }

        private bool IsInfoAsExpected(byte deltaTime, byte metaEventCode, byte messageCode, byte data)
        {
            return deltaTime == this._expectedDeltaTime && metaEventCode == this._expectedMetaEventCode && messageCode == this._expectedMessageCode && data == this._expectedData;
        }

        public bool ReadEndOfTrackMessage(BinaryReader reader)
        {
            if (reader == null || reader.BaseStream.Length - reader.BaseStream.Position < 4)
                return false;
            var deltaTime = reader.ReadByte();
            var metaEventCode = reader.ReadByte();
            var messageCode = reader.ReadByte();
            var data = reader.ReadByte();
            return (IsInfoAsExpected(deltaTime, metaEventCode, messageCode, data));
        }
    }
}
