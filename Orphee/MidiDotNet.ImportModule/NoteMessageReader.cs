using System.Collections.Generic;
using System.IO;
using Midi;
using MidiDotNet.ImportModule.Interfaces;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ImportModule
{
    public class NoteMessageReader : INoteMessageReader
    {
        private readonly IDeltaTimeReader _deltaTimeReader;
        private readonly IEndOfTrackMessageReader _endOfTrackMessageReader;
        private readonly byte _expectedNoteOnMessageCode;
        private readonly byte _expectedNoteOffMessageCode;
        private readonly int _maxExpectedDeltaTime;
        private readonly int _maxExpectedNoteIndex;
        public IList<IOrpheeNoteMessage> OrpheeNoteMessageList { get; private set; }

        public NoteMessageReader(IDeltaTimeReader deltaTimeReader, IEndOfTrackMessageReader endOfTrackMessageReader)
        {
            this._deltaTimeReader = deltaTimeReader;
            this._expectedNoteOnMessageCode = 0x90;
            this._expectedNoteOffMessageCode = 0x80;
            this._maxExpectedDeltaTime = 2097151;
            this._maxExpectedNoteIndex = 127;
            this._endOfTrackMessageReader = endOfTrackMessageReader;
        }

        private bool IsInfoAsExpected(int deltaTime, byte messageCode, byte noteIndex)
        {
            return ((messageCode & 0x90) == this._expectedNoteOnMessageCode || (messageCode & 0x80) == this._expectedNoteOffMessageCode) && deltaTime <= this._maxExpectedDeltaTime && noteIndex <= this._maxExpectedNoteIndex;
        }

        public bool ReadNoteMessage(BinaryReader reader, uint trackLength)
        {
            this.OrpheeNoteMessageList = new List<IOrpheeNoteMessage>();
            if (reader == null || trackLength < 4)
                return false;
            while (trackLength > 0)
            {
                var deltaTime = this._deltaTimeReader.GetIntDeltaTime(reader);
                var messageCode = reader.ReadByte();
                var noteIndex = reader.ReadByte();
                var velocity = reader.ReadByte();
                if (!IsInfoAsExpected(deltaTime, messageCode, noteIndex))
                    return false;
                AddNewNoteMessageToOrpheeNoteMessageList(deltaTime, messageCode, noteIndex, velocity);
                trackLength -= (uint) (deltaTime > 127 ? (deltaTime > 16383 ? 6 : 5) : 4);
                if (trackLength == 4)
                    return this._endOfTrackMessageReader.ReadEndOfTrackMessage(reader);
            }
            return true;
        }

        private void AddNewNoteMessageToOrpheeNoteMessageList(int deltaTime, byte messageCode, byte noteIndex, byte velocity)
        {
            var channel = 0;

            if ((messageCode & 0x90) == 0x90)
                channel = messageCode ^ 0x90;
            else
                channel = messageCode ^ 0x80;

            var newOrpheeNoteMessage = new OrpheeNoteMessage()
            {
                Channel = channel,
                DeltaTime = deltaTime,
                MessageCode = messageCode,
                Note = (Note) noteIndex,
                Velocity = velocity
            };

            this.OrpheeNoteMessageList.Add(newOrpheeNoteMessage);
        }
    }
}
