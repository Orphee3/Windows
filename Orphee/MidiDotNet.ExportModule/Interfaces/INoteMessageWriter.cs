using System.Collections.Generic;
using System.IO;
using Midi;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ExportModule.Interfaces
{
    /// <summary>
    ///  NoteMessageWriter interface.
    /// </summary>
    public interface INoteMessageWriter
    {
        // Properties

        // Methods
        /// <summary>
        /// Function writting the NoteMessage in the MIDI
        /// files.
        /// </summary>
        /// <param name="writer">Instance of the BinaryWriter class writting the noteMessage event in the MIDI file</param>
        /// <param name="orpheeNoteMessageList">List of OrpheeNoteMessage containing all the noteMessages to write</param>
        /// <param name="channel">Value representing the Channel related to the actual track for which the noteMessages are written</param>
        /// <param name="instrument">Instrument related to the actual track</param>
        void WriteNoteMessages(BinaryWriter writer, IList<IOrpheeNoteMessage> orpheeNoteMessageList, int channel, Instrument instrument);
    }
}
