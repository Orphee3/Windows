using System.IO;
using System.Text;
using MidiDotNet.IOModules.ExportToOrpheeFileModule.Interfaces;
using MidiDotNet.IOModules.Interfaces;

namespace MidiDotNet.IOModules.ExportToOrpheeFileModule
{
    public class FileHeaderWriter : IFileHeaderWriter
    {
        public bool Write(BinaryWriter writer, IOrpheeFileParameters orpheeFileParameters)
        {
            if (writer == null)
                return false;
            writer.Write(Encoding.UTF8.GetBytes("MThd"));
            writer.Write(Utils.Instance.SwapUInt32(6));
            writer.Write(Utils.Instance.SwapUInt16(orpheeFileParameters.OrpheeFileType));
            writer.Write(Utils.Instance.SwapUInt16(orpheeFileParameters.NumberOfTracks));
            writer.Write(Utils.Instance.SwapUInt16(orpheeFileParameters.DeltaTicksPerQuarterNote));
            return true;
        }
    }
}
