using System;
using System.IO;
using MidiDotNet.ExportModule.Interfaces;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ExportModule
{
    public class TimeSignatureMessageWriter : ITimeSignatureMessageWriter
    {
        public void WriteTimeSignatureMessage(BinaryWriter writer, IPlayerParameters playerParameters)
        {
            writer.Write((byte) 0x00);
            writer.Write((byte) 0xFF);
            writer.Write((byte) 0x58);
            writer.Write((byte) 4);
            writer.Write((byte) playerParameters.TimeSignatureNominator);
            writer.Write((byte) Math.Log(playerParameters.TimeSignatureDenominator, 2));
            writer.Write((byte) playerParameters.TimeSignatureClocksPerBeat);
            writer.Write((byte) (playerParameters.TimeSignatureNumberOf32ThNotePerBeat * 2));
        }
    }
}
