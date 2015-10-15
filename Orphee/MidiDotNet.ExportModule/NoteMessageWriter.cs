using System.Collections.Generic;
using System.IO;
using Midi;
using MidiDotNet.ExportModule.Interfaces;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ExportModule
{
    /// <summary>
    /// Class containing the functions writting the noteMessage
    /// in the MIDI file.
    /// </summary>
    public class NoteMessageWriter : INoteMessageWriter
    {
        private readonly IProgramChangeMessageWriter _programChangeMessageWriter;
        private readonly IEndOfTrackMessageWriter _endOfTrackMessageWriter;

        /// <summary>
        /// Constructor initializing programChangeMessageWriter and 
        /// endOfTrackMessageWritter through dependency injection
        /// </summary>
        /// <param name="programChangeMessageWriter">Instance of the ProgramChangeMessageWriter class used to write the programChange messages in the MIDI file</param>
        /// <param name="endOfTrackMessageWriter">Instance of the EndOfTrackWriter class used to write the endOfTrack messages at the end of every track of the MIDI file</param>
        public NoteMessageWriter(IProgramChangeMessageWriter programChangeMessageWriter, IEndOfTrackMessageWriter endOfTrackMessageWriter)
        {
            this._programChangeMessageWriter = programChangeMessageWriter;
            this._endOfTrackMessageWriter = endOfTrackMessageWriter;
        }

        /// <summary>
        /// Function writting the NoteMessage in the MIDI
        /// files.
        /// </summary>
        /// <param name="writer">Instance of the BinaryWriter class writting the noteMessage event in the MIDI file</param>
        /// <param name="orpheeNoteMessageList">List of OrpheeNoteMessage containing all the noteMessages to write</param>
        /// <param name="channel">Value representing the Channel related to the actual track for which the noteMessages are written</param>
        /// <param name="instrument">Instrument related to the actual track</param>
        public void WriteNoteMessages(BinaryWriter writer, IList<IOrpheeNoteMessage> orpheeNoteMessageList, int channel, Instrument instrument)
        {
            var deltaTimeWriter = new DeltaTimeWriter();

            this._programChangeMessageWriter.WriteProgramChangeMessage(writer, channel, instrument);

            foreach (var orpheeNoteMessage in orpheeNoteMessageList)
            {
                deltaTimeWriter.WriteDeltaTime(writer, orpheeNoteMessage.DeltaTime);
                writer.Write((byte) (orpheeNoteMessage.MessageCode + orpheeNoteMessage.Channel));
                writer.Write((byte) orpheeNoteMessage.Note);
                writer.Write((byte) orpheeNoteMessage.Velocity);
            }
            this._endOfTrackMessageWriter.WriteEndOfTrackMessage(writer);
        }
    }
}
