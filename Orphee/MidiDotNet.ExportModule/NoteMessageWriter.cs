using System.Collections.Generic;
using System.IO;
using Midi;
using MidiDotNet.ExportModule.Interfaces;
using Orphee.CreationShared.Interfaces;
using IEndOfTrackMessageWriter = MidiDotNet.ExportModule.Interfaces.IEndOfTrackMessageWriter;

namespace MidiDotNet.ExportModule
{
    public class NoteMessageWriter : INoteMessageWriter
    {
        private readonly IProgramChangeMessageWriter _programChangeMessageWriter;
        private readonly IEndOfTrackMessageWriter _endOfTrackMessageWriter;

        public NoteMessageWriter(IProgramChangeMessageWriter programChangeMessageWriter, IEndOfTrackMessageWriter endOfTrackMessageWriter)
        {
            this._programChangeMessageWriter = programChangeMessageWriter;
            this._endOfTrackMessageWriter = endOfTrackMessageWriter;
        }
        
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
