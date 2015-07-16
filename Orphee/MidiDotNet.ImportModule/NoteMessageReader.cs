using System.IO;
using MidiDotNet.ImportModule.Interfaces;

namespace MidiDotNet.ImportModule
{
    public class NoteMessageReader : INoteMessageReader
    {
        private readonly IDeltaTimeReader _deltaTimeReader;
        private readonly byte _expectedNoteOnMessageCode;
        private readonly byte _expectedNoteOffMessageCode;
        private readonly int _maxExpectedDeltaTime;
        private readonly int _maxExpectedNoteIndex;
        public int DeltaTime { get; private set; }
        public int NoteIndex { get; private set; }
        public int Velocity { get; private set; }

        public NoteMessageReader(IDeltaTimeReader deltaTimeReader)
        {
            this._deltaTimeReader = deltaTimeReader;
            this._expectedNoteOnMessageCode = 0x90;
            this._expectedNoteOffMessageCode = 0x80;
            this._maxExpectedDeltaTime = 2097151;
            this._maxExpectedNoteIndex = 127;
        }

        private bool IsInfoAsExpected(byte messageCode)
        {
            return ((messageCode & 0x90) == this._expectedNoteOnMessageCode || (messageCode & 0x80) == this._expectedNoteOffMessageCode) && this.DeltaTime <= this._maxExpectedDeltaTime && this.NoteIndex <= this._maxExpectedNoteIndex;
        }

        public bool ReadNoteMessage(BinaryReader reader)
        {
            if (reader == null || reader.BaseStream.Length - reader.BaseStream.Position < 4)
                return false;
            this.DeltaTime = this._deltaTimeReader.GetIntDeltaTime(reader);
            var mesageCode = reader.ReadByte();
            this.NoteIndex = reader.ReadByte();
            this.Velocity = reader.ReadByte();
            
            return IsInfoAsExpected(mesageCode);
        }
    }
}
