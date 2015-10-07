using System.IO;
using System.Text;
using Midi;
using MidiDotNet.ImportModule.Interfaces;
using MidiDotNet.Shared.Interfaces;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ImportModule
{
    public class TrackHeaderReader : ITrackHeaderReader
    {
        public ITimeSignatureMessageReader TimeSignatureMessageReader { get; private set; }
        public ITempoMessageReader TempoMessageReader { get; private set; }
        public IProgramChangeMessageReader ProgramChangeMessageReader { get; private set; }
        public IPlayerParameters PlayerParameters { get; private set; }
        private readonly ISwapManager _swapManager;
        public uint TrackLength { get; private set; }

        public TrackHeaderReader(ISwapManager swapManager, ITimeSignatureMessageReader timeSignatureMessageReader, ITempoMessageReader tempoMessageReader, IProgramChangeMessageReader programChangeMessageReader)
        {
            this._swapManager = swapManager;
            this.TimeSignatureMessageReader = timeSignatureMessageReader;
            this.TempoMessageReader = tempoMessageReader;
            this.ProgramChangeMessageReader = programChangeMessageReader;
        }

        private bool ReadOtherMessages(BinaryReader reader)
        {
            if (!this.TimeSignatureMessageReader.ReadTimeSignatureMessage(reader) || !this.TempoMessageReader.ReadTempoMessage(reader))
                return false;
            this.PlayerParameters = new PlayerParameters()
            {
                TimeSignatureNominator = this.TimeSignatureMessageReader.Nominator,
                TimeSignatureDenominator = this.TimeSignatureMessageReader.Denominator,
                TimeSignatureClocksPerBeat = this.TimeSignatureMessageReader.ClocksPerBeat,
                TimeSignatureNumberOf32ThNotePerBeat = this.TimeSignatureMessageReader.NumberOf32ThNotePerBeat,
                Tempo = this.TempoMessageReader.Tempo
            };
            this.TrackLength -= 15;
            return true;
        }

        public bool ReadTrackHeader(BinaryReader reader, int trackPos)
        {
            var result = true;
            if (reader == null || reader.BaseStream.Length - reader.BaseStream.Position < 8)
                return false;
            var header = Encoding.UTF8.GetString(reader.ReadBytes(4), 0, 4);
            this.TrackLength = this._swapManager.SwapUInt32(reader.ReadUInt32());
            if (trackPos == 0)
                result = ReadOtherMessages(reader);
            if (!this.ProgramChangeMessageReader.ReadProgramChangeMessage(reader))
                return false;
            this.TrackLength -= 3;
            return header == "MTrk" && result;
        }
    }
}