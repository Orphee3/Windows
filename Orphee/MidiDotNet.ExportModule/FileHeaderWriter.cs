using System.IO;
using System.Text;
using MidiDotNet.ExportModule.Interfaces;
using MidiDotNet.Shared.Interfaces;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ExportModule
{
    public class FileHeaderWriter : IFileHeaderWriter
    {
        private readonly ISwapManager _swapManager;

        public FileHeaderWriter(ISwapManager swapManager)
        {
            this._swapManager = swapManager;
        }

        public bool WriteFileHeader(BinaryWriter writer, IOrpheeFileParameters orpheeFileParameters)
        {
            if (writer == null)
                return false;
            writer.Write(Encoding.UTF8.GetBytes("MThd"));
            writer.Write(this._swapManager.SwapUInt32(6));
            writer.Write(this._swapManager.SwapUInt16(orpheeFileParameters.OrpheeFileType));
            writer.Write(this._swapManager.SwapUInt16(orpheeFileParameters.NumberOfTracks));
            writer.Write(this._swapManager.SwapUInt16(orpheeFileParameters.DeltaTicksPerQuarterNote));
            return true;
        }
    }
}
