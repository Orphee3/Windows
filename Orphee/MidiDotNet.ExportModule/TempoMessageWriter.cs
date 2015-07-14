using System;
using System.IO;
using MidiDotNet.ExportModule.Interfaces;

namespace MidiDotNet.ExportModule
{
    public class TempoMessageWriter : ITempoMessageWriter
    {
        public void WriteTempoMessage(BinaryWriter writer, uint tempo)
        {
            writer.Write((byte) 0x00);
            writer.Write((byte) 0xFF);
            writer.Write((byte) 0x51);
            writer.Write((byte) 0x03);
            
            var tempoToByteArray = ConvertTempoToByteArray(60000000 / tempo);
            writer.Write(tempoToByteArray[1]);
            writer.Write(tempoToByteArray[2]);
            writer.Write(tempoToByteArray[3]);
        }

        public byte[] ConvertTempoToByteArray(uint tempo)
        {
            var intBytes = BitConverter.GetBytes(tempo);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(intBytes);
            return (intBytes);
        }
    }
}
