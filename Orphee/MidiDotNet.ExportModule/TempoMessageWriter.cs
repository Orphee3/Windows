using System;
using System.IO;
using MidiDotNet.ExportModule.Interfaces;

namespace MidiDotNet.ExportModule
{
    /// <summary>
    /// Class containing the needed function to write tempoMessages
    /// in the MIDI file
    /// </summary>
    public class TempoMessageWriter : ITempoMessageWriter
    {
        /// <summary>
        /// Function writting the tempoMessage
        /// in the MIDI file
        /// </summary>
        /// <param name="writer">Instance of the BinaryWriter class writting the noteMessages in the MIDI file</param>
        /// <param name="tempo">Value representing the tempo value to be written</param>
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

        private byte[] ConvertTempoToByteArray(uint tempo)
        {
            var intBytes = BitConverter.GetBytes(tempo);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(intBytes);
            return (intBytes);
        }
    }
}
