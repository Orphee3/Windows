using System.IO;
using System.Text;
using MidiDotNet.ImportModule.Interfaces;
using MidiDotNet.Shared.Interfaces;

namespace MidiDotNet.ImportModule
{
    /// <summary>
    /// Class containing all the function needed in order
    /// to read the MIDI file header
    /// </summary>
    public class FileHeaderReader : IFileHeaderReader
    {
        private readonly ISwapManager _swapManager;
        private readonly string _expectedHeader;
        private readonly byte _expectedHeaderLength;
        /// <summary>Value representing the the actual file type of the MIDI file </summary>
        public ushort FileType { get; private set; }
        /// <summary>Value representing the number of tracks contained in the MIDI file </summary>
        public ushort NumberOfTracks { get; private set; }
        /// <summary>Value representing the delta ticks per quarter notes </summary>
        public ushort DeltaTicksPerQuarterNote { get; private set; }

        /// <summary>
        /// Constructor initializing swapManager through
        /// dependency injection
        /// </summary>
        /// <param name="swapManager">Instance of the SwapManager class used to swap the position of the bytes contained in the value it's given</param>
        public FileHeaderReader(ISwapManager swapManager)
        {
            this._swapManager = swapManager;
            this._expectedHeader = "MThd";
            this._expectedHeaderLength = 6;
        }

        /// <summary>
        /// Function reading the MIDI file header
        /// </summary>
        /// <param name="reader">Instance of the BinaryReader class read the noteMessage in the MIDI file</param>
        /// <returns>Returns true if the message was read correctly and false if it wasn't</returns>
        public bool ReadFileHeader(BinaryReader reader)
        {
            if (reader == null || reader.BaseStream.Length - reader.BaseStream.Position < 14)
                return false;
            var header = Encoding.UTF8.GetString(reader.ReadBytes(4), 0, 4);
            var headerLength = this._swapManager.SwapUInt32(reader.ReadUInt32());
            this.FileType = this._swapManager.SwapUInt16(reader.ReadUInt16());
            this.NumberOfTracks = this._swapManager.SwapUInt16(reader.ReadUInt16());
            this.DeltaTicksPerQuarterNote = this._swapManager.SwapUInt16(reader.ReadUInt16());
            return IsInfoAsExpected(header, headerLength);
        }

        private bool IsInfoAsExpected(string header, uint trackLength)
        {
            return header == this._expectedHeader && trackLength == this._expectedHeaderLength && this.NumberOfTracks <= 15 && this.FileType <= 2;
        }
    }
}
