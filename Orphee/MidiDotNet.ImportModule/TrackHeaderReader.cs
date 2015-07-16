using System.IO;
using System.Text;
using MidiDotNet.ImportModule.Interfaces;
using MidiDotNet.Shared.Interfaces;

namespace MidiDotNet.ImportModule
{
    public class TrackHeaderReader : ITrackHeaderReader
    {
        private readonly ISwapManager _swapManager;
        public uint TrackLength { get; private set; }

        public TrackHeaderReader(ISwapManager swapManager)
        {
            this._swapManager = swapManager;
        }

        public bool ReadTrackHeader(BinaryReader reader)
        {
            if (reader == null || reader.BaseStream.Length - reader.BaseStream.Position < 8)
                return false;
            var header = Encoding.UTF8.GetString(reader.ReadBytes(4), 0, 4);
            this.TrackLength = this._swapManager.SwapUInt32(reader.ReadUInt32());
            return header == "MTrk";
        }
    }
}
