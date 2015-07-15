using System.IO;
using Midi;
using MidiDotNet.ExportModule.Interfaces;

namespace MidiDotNet.ExportModule
{
    public class ProgramChangeMessageWriter : IProgramChangeMessageWriter
    {
        public void WriteProgramChangeMessage(BinaryWriter writer, int channel, Instrument instrument)
        {
            writer.Write((byte) 0x00);
            writer.Write((byte) (0xC0 + (byte) channel));
            writer.Write((byte) instrument);
        }
    }
}
