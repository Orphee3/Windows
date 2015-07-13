using System.IO;
using MidiDotNet.ExportModule.Interfaces;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ExportModule
{
    public class TrackHeaderWriter : ITrackHeaderWriter
    {
        private readonly ITimeSignatureMessageWriter _timeSignatureMessageWriter;
        private readonly ITempoMessageWriter _tempoMessageWriter;
        private readonly IProgramMessageWriter _programMessageWriter;

        public TrackHeaderWriter(ITimeSignatureMessageWriter timeSignatureMessageWriter, ITempoMessageWriter tempoMessageWritern, IProgramMessageWriter programMessageWriter)
        {
            this._timeSignatureMessageWriter = timeSignatureMessageWriter;
            this._tempoMessageWriter = tempoMessageWritern;
            this._programMessageWriter = programMessageWriter;
        }

        public bool WriteTrackHeader(BinaryWriter writer, IPlayerParameters playerParameters, int trackPos)
        {
            if (writer == null)
                return false;
            return true;
        }
    }
}
