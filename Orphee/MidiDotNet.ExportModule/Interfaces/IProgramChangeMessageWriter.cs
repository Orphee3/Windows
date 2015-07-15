using System.IO;
using Midi;

namespace MidiDotNet.ExportModule.Interfaces
{
    public interface IProgramChangeMessageWriter
    {
        // Properties

        // Methods
        void WriteProgramChangeMessage(BinaryWriter writer, int channel, Instrument instrument);
    }
}
