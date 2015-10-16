using System.IO;
using MidiDotNet.ImportModule.Interfaces;

namespace MidiDotNet.ImportModule
{
    /// <summary>
    /// Class containing all the needed functions in order to read
    /// the endOfTrack message at the end of each track.
    /// </summary>
    public class EndOfTrackMessageReader : IEndOfTrackMessageReader
    {
        private readonly byte _expectedDeltaTime;
        private readonly byte _expectedMetaEventCode;
        private readonly byte _expectedMessageCode;
        private readonly byte _expectedData;

        /// <summary>
        /// Constructor
        /// </summary>
        public EndOfTrackMessageReader()
        {
            this._expectedDeltaTime = 0;
            this._expectedMetaEventCode = 0xFF;
            this._expectedMessageCode = 0x2F;
            this._expectedData = 0;
        }

        /// <summary>
        /// Function reading the endOfTrack messagage at the end
        /// of each track.
        /// </summary>
        /// <param name="reader">Instance of the BinaryReader class read the noteMessage in the MIDI filed</param>
        /// <returns>Returns true if the message was read correctly and false if it wasn't</returns>
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

        private bool IsInfoAsExpected(byte deltaTime, byte metaEventCode, byte messageCode, byte data)
        {
            return deltaTime == this._expectedDeltaTime && metaEventCode == this._expectedMetaEventCode && messageCode == this._expectedMessageCode && data == this._expectedData;
        }
    }
}
