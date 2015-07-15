using System.IO;
using System.Text;
using MidiDotNet.ExportModule.Interfaces;
using MidiDotNet.Shared.Interfaces;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ExportModule
{
    public class TrackHeaderWriter : ITrackHeaderWriter
    {
        private readonly ITimeSignatureMessageWriter _timeSignatureMessageWriter;
        private readonly ITempoMessageWriter _tempoMessageWriter;
        private readonly ISwapManager _swapManager;

        public TrackHeaderWriter(ITimeSignatureMessageWriter timeSignatureMessageWriter, ITempoMessageWriter tempoMessageWritern, ISwapManager swapManager)
        {
            this._timeSignatureMessageWriter = timeSignatureMessageWriter;
            this._tempoMessageWriter = tempoMessageWritern;
            this._swapManager = swapManager;
        }

        public bool WriteTrackHeader(BinaryWriter writer, IPlayerParameters playerParameters, uint trackLength)
        {
            if (writer == null)
                return false;
            writer.Write(Encoding.UTF8.GetBytes("MTrk"));
            writer.Write(this._swapManager.SwapUInt32(trackLength));
            if (playerParameters != null)
                WritePlayerParameters(writer, playerParameters);
            return true;
        }

        private void WritePlayerParameters(BinaryWriter writer, IPlayerParameters playerParameters)
        {
            this._timeSignatureMessageWriter.WriteTimeSignatureMessage(writer, playerParameters);
            this._tempoMessageWriter.WriteTempoMessage(writer, playerParameters.Tempo);
        }
    }
}
