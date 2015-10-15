using System;
using System.IO;
using MidiDotNet.ExportModule.Interfaces;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ExportModule
{
    /// <summary>
    /// Class containing all the needed function
    /// in order to write the timeSignatureMessage in 
    /// the MIDI file
    /// </summary>
    public class TimeSignatureMessageWriter : ITimeSignatureMessageWriter
    {
        /// <summary>
        /// Function writting the timeSignatureMessage
        /// in the MIDI file
        /// </summary>
        /// <param name="writer">Instance of the BinaryWriter class writting the noteMessages in the MIDI file</param>
        /// <param name="playerParameters">Instance of the PlayerParameters containing all the data needed to create the timeSignatureMessage to be written</param>
        public void WriteTimeSignatureMessage(BinaryWriter writer, IPlayerParameters playerParameters)
        {
            writer.Write((byte) 0x00);
            writer.Write((byte) 0xFF);
            writer.Write((byte) 0x58);
            writer.Write((byte) 0x04);
            writer.Write((byte) playerParameters.TimeSignatureNominator);
            writer.Write((byte) Math.Log(playerParameters.TimeSignatureDenominator, 2));
            writer.Write((byte) playerParameters.TimeSignatureClocksPerBeat);
            writer.Write((byte) (playerParameters.TimeSignatureNumberOf32ThNotePerBeat * 2));
        }
    }
}
