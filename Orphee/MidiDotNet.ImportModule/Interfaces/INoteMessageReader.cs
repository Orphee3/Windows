using System.Collections.Generic;
using System.IO;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ImportModule.Interfaces
{
    public interface INoteMessageReader
    {
        // Properties
        IList<IOrpheeNoteMessage> OrpheeNoteMessageList { get; } 

        // Methods
        bool ReadNoteMessage(BinaryReader reader, uint trackLength);
    }
}
