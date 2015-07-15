using System.Collections.Generic;
using System.IO;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ExportModule.Interfaces
{
    public interface INoteMessageWriter
    {
        // Properties

        // Methods
        void WriteNoteMessages(BinaryWriter writer, IList<IOrpheeNoteMessage> orpheeNoteMessageList);
    }
}
