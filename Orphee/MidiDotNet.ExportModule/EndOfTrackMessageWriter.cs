using System.IO;
using Orphee.CreationShared.Interfaces;
using IEndOfTrackMessageWriter = MidiDotNet.ExportModule.Interfaces.IEndOfTrackMessageWriter;

namespace MidiDotNet.ExportModule
{
    public class EndOfTrackMessageWriter : IEndOfTrackMessageWriter
    {
        public void WriteEndOfTrackMessage(BinaryWriter writer)
        {
            writer.Write((byte) 0x00);
            writer.Write((byte) 0xFF);
            writer.Write((byte) 0x2F);
            writer.Write((byte) 0x00);
        }
    }
}
