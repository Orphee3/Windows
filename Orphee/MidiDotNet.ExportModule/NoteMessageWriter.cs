using System.Collections.Generic;
using System.IO;
using MidiDotNet.ExportModule.Interfaces;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ExportModule
{
    public class NoteMessageWriter : INoteMessageWriter
    {
        public void WriteNoteMessages(BinaryWriter writer, IList<IOrpheeNoteMessage> orpheeNoteMessageList)
        {
            var deltaTimeWriter = new DeltaTimeWriter();
            
            foreach (var orpheeNoteMessage in orpheeNoteMessageList)
            {
                deltaTimeWriter.WriteDeltaTime(writer, orpheeNoteMessage.DeltaTime);
                writer.Write((byte) (orpheeNoteMessage.MessageCode + orpheeNoteMessage.Channel));
                writer.Write((byte) orpheeNoteMessage.Note);
                writer.Write((byte) orpheeNoteMessage.Velocity);
            }
        }
    }
}
