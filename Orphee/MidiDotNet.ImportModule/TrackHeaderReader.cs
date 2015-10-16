using System.IO;
using System.Text;
using MidiDotNet.ImportModule.Interfaces;
using MidiDotNet.Shared.Interfaces;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ImportModule
{
    /// <summary>
    /// Class containing all the needed function on order to read the
    /// track header of each track of the MIDI file
    /// </summary>
    public class TrackHeaderReader : ITrackHeaderReader
    {
        /// <summary>Instance of the TimeSignatureMessageReader class</summary>
        public ITimeSignatureMessageReader TimeSignatureMessageReader { get; private set; }
        /// <summary>Instance of the TempoMessageReader class</summary>
        public ITempoMessageReader TempoMessageReader { get; private set; }
        /// <summary>Instance of the ProgramChangerMessageReader class</summary>
        public IProgramChangeMessageReader ProgramChangeMessageReader { get; private set; }
        /// <summary>Instance of the PlayerParameter class</summary>
        public IPlayerParameters PlayerParameters { get; private set; }
        /// <summary>Value representing the track length of the actual track </summary>
        public uint TrackLength { get; private set; }
        private readonly ISwapManager _swapManager;

        /// <summary>
        /// Constructor initializing swapManager, timeSignatureMessageReader,
        /// tempoMessageReader and programChangeMessageReader through dependency injection
        /// </summary>
        /// <param name="swapManager">Instance of the SwapManager class used to swap the position of the bytes contained in the value it's given</param>
        /// <param name="timeSignatureMessageReader">Instance of the TimeSignatureMessagReader class used to read the timeSignatureMessage in the MIDI file</param>
        /// <param name="tempoMessageReader">Instance of the TempoMessageReader class used to read the tempoMessages in the MIDI file</param>
        /// <param name="programChangeMessageReader">Instance of the ProgramChangeMessageReader class used to read the programCHangeMessages in the MIDI file</param>
        public TrackHeaderReader(ISwapManager swapManager, ITimeSignatureMessageReader timeSignatureMessageReader, ITempoMessageReader tempoMessageReader, IProgramChangeMessageReader programChangeMessageReader)
        {
            this._swapManager = swapManager;
            this.TimeSignatureMessageReader = timeSignatureMessageReader;
            this.TempoMessageReader = tempoMessageReader;
            this.ProgramChangeMessageReader = programChangeMessageReader;
        }

        /// <summary>
        /// Function reading the track header of each track
        /// found in the MIDI file
        /// </summary>
        /// <param name="reader">Instance of the BinaryReader class read the noteMessage in the MIDI file</param>
        /// <param name="trackPos">Value representing the track position in the MIDI file</param>
        /// <returns>Returns true if the message was read correctly and false if it wasn't</returns>
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
    }
}