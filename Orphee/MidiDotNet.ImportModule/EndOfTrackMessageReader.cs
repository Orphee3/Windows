using System.IO;
using MidiDotNet.ImportModule.Interfaces;

namespace MidiDotNet.ImportModule
{
    public class EndOfTrackMessageReader : IEndOfTrackMessageReader
    {
        private byte _deltaTime;
        private byte _metaEventCode;
        private byte _messageCode;
        private byte _data;

        private bool IsInfoAsExpected()
        {
            return this._deltaTime == 0 && this._metaEventCode == 0xFF && this._messageCode == 0x2F && this._data == 0;
        }

        public bool ReadEndOfTrackMessage(BinaryReader reader)
        {
            if (reader == null)
                return false;
            this._deltaTime = reader.ReadByte();
            this._metaEventCode = reader.ReadByte();
            this._messageCode = reader.ReadByte();
            this._data = reader.ReadByte();
            return (IsInfoAsExpected());
        }
    }
}
