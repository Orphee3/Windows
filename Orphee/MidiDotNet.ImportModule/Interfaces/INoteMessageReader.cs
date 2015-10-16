using System.Collections.Generic;
using System.IO;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ImportModule.Interfaces
{
    /// <summary>
    /// NoteMessageReader interface.
    /// </summary>
    public interface INoteMessageReader
    {
        // Properties

        /// <summary>List of OrpheeNoteMessages representing the track content </summary>
        IList<IOrpheeNoteMessage> OrpheeNoteMessageList { get; }

        // Methods

        /// <summary>
        /// Function reading the noteMessages contained in the
        /// track
        /// </summary>
        /// <param name="reader">Instance of the BinaryReader class read the noteMessage in the MIDI file</param>
        /// <param name="trackLength">Value representing the actual track length</param>
        /// <returns>Returns true if the message was read correctly and false if it wasn't</returns>
        bool ReadNoteMessage(BinaryReader reader, uint trackLength);
    }
}
