using System.IO;
using System.Text;
using MidiDotNet.ExportModule.Interfaces;
using MidiDotNet.Shared.Interfaces;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ExportModule
{
    /// <summary>
    /// Class containing all the needed functions in order
    /// to write the track header of the given track in the MIDI
    /// file.
    /// </summary>
    public class TrackHeaderWriter : ITrackHeaderWriter
    {
        private readonly ITimeSignatureMessageWriter _timeSignatureMessageWriter;
        private readonly ITempoMessageWriter _tempoMessageWriter;
        private readonly ISwapManager _swapManager;

        /// <summary>
        /// Conctructor initializing the variables timeSignatureMessageWriter,
        /// tempoMessageWritern and swapManager through dependency injection 
        /// </summary>
        /// <param name="timeSignatureMessageWriter">Instance of the TimeSignatureMessagWriter class used to write the timeSignatureMessage in the MIDI file</param>
        /// <param name="tempoMessageWriter">Instance of the TempoMessageWriter class used to write the tempoMessages in the MIDI file</param>
        /// <param name="swapManager">Instance of the SwapManager class used to swap the position of the bytes contained in the value it's given</param>
        public TrackHeaderWriter(ITimeSignatureMessageWriter timeSignatureMessageWriter, ITempoMessageWriter tempoMessageWriter, ISwapManager swapManager)
        {
            this._timeSignatureMessageWriter = timeSignatureMessageWriter;
            this._tempoMessageWriter = tempoMessageWriter;
            this._swapManager = swapManager;
        }

        /// <summary>
        /// Function writting the track header of every track
        /// </summary>
        /// <param name="writer">Instance of the BinaryWriter class writting the noteMessages in the MIDI file</param>
        /// <param name="playerParameters">Instance of the PlayerParameters containing all the data needed to create the timeSignatureMessage to be written</param>
        /// <param name="trackLength">Value representing the length of the processed track</param>
        /// <returns>Returns true if the message has been written and false if it hasn't</returns>
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
