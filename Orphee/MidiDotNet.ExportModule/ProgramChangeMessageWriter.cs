using System.IO;
using MidiDotNet.ExportModule.Interfaces;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ExportModule
{
    public class ProgramChangeMessageWriter : IProgramChangeMessageWriter
    {
        public void WriteProgramChangeMessage(BinaryWriter writer, IOrpheeTrack orpheeTrack)
        {
            writer.Write((byte) 0x00);
            writer.Write((byte) (0xC0 + (byte)orpheeTrack.Channel));
            writer.Write((byte) orpheeTrack.CurrentInstrument);
        }
    }
}
