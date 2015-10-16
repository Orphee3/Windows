using System.Collections.Generic;
using System.IO;
using Midi;
using MidiDotNet.ImportModule.Interfaces;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ImportModule
{
    /// <summary>
    /// Class containing all the functions needed in order to read
    /// the noteMessages contained in the actual track
    /// </summary>
    public class NoteMessageReader : INoteMessageReader
    {
        private readonly IDeltaTimeReader _deltaTimeReader;
        private readonly IEndOfTrackMessageReader _endOfTrackMessageReader;
        private readonly byte _expectedNoteOnMessageCode;
        private readonly byte _expectedNoteOffMessageCode;
        private readonly int _maxExpectedDeltaTime;
        private readonly int _maxExpectedNoteIndex;
        /// <summary>List of OrpheeNoteMessages representing the track content </summary>
        public IList<IOrpheeNoteMessage> OrpheeNoteMessageList { get; private set; }

        /// <summary>
        /// Constructor initializing deltaTimeReader and 
        /// endOfTrackMessageReader through dependency injection
        /// </summary>
        /// <param name="deltaTimeReader">Instance of the DeltaTimeReader class used to read the delta time od each message contained in the MIDI file</param>
        /// <param name="endOfTrackMessageReader">Instance of the EndOfTrackReader used to read the endOfTrackMessage that can be found at the end of each track</param>
        public NoteMessageReader(IDeltaTimeReader deltaTimeReader, IEndOfTrackMessageReader endOfTrackMessageReader)
        {
            this._deltaTimeReader = deltaTimeReader;
            this._expectedNoteOnMessageCode = 0x90;
            this._expectedNoteOffMessageCode = 0x80;
            this._maxExpectedDeltaTime = 2097151;
            this._maxExpectedNoteIndex = 127;
            this._endOfTrackMessageReader = endOfTrackMessageReader;
        }

        /// <summary>
        /// Function reading the noteMessages contained in the
        /// track
        /// </summary>
        /// <param name="reader">Instance of the BinaryReader class read the noteMessage in the MIDI file</param>
        /// <param name="trackLength">Value representing the actual track length</param>
        /// <returns>Returns true if the message was read correctly and false if it wasn't</returns>
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
            int channel;

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

        private bool IsInfoAsExpected(int deltaTime, byte messageCode, byte noteIndex)
        {
            return ((messageCode & 0x90) == this._expectedNoteOnMessageCode || (messageCode & 0x80) == this._expectedNoteOffMessageCode) && deltaTime <= this._maxExpectedDeltaTime && noteIndex <= this._maxExpectedNoteIndex;
        }
    }
}
