using System.IO;
using System.Text;
using MidiDotNet.ImportModule.Interfaces;
using MidiDotNet.Shared.Interfaces;

namespace MidiDotNet.ImportModule
{
    public class FileHeaderReader : IFileHeaderReader
    {
        private readonly ISwapManager _swapManager;
        private readonly string _expectedHeader;
        private readonly byte _expectedHeaderLength;
        public ushort FileType { get; private set; }
        public ushort NumberOfTracks { get; private set; }
        public ushort DeltaTicksPerQuarterNote { get; private set; }

        public FileHeaderReader(ISwapManager swapManager)
        {
            this._swapManager = swapManager;
            this._expectedHeader = "MThd";
            this._expectedHeaderLength = 6;
        }

        private bool IsInfoAsExpected(string header, uint trackLength)
        {
            return header == this._expectedHeader && trackLength == this._expectedHeaderLength && this.NumberOfTracks <= 15 && this.FileType <= 2;
        }

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
    }
}
