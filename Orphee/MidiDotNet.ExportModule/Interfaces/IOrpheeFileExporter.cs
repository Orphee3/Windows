using System.Threading.Tasks;
using Orphee.CreationShared.Interfaces;

namespace MidiDotNet.ExportModule.Interfaces
{
    /// <summary>
    ///  OrpheeFileExporter interface.
    /// </summary>
    public interface IOrpheeFileExporter
    {
        // Properties

        // Methods

        /// <summary>
        /// Function converting view NoteMap to a list of noteMessage usable by
        /// the NoteMessageWriter class
        /// </summary>
        /// <param name="orpheeFile">Instance of the OrpheeFile class containing the graphical representation of the noteMessage messages</param>
        void ConvertTracksNoteMapToOrpheeNoteMessageList(IOrpheeFile orpheeFile);
        /// <summary>
        /// Function saving the actual piece to the MIDI file
        /// </summary>
        /// <param name="orpheeFile">Instance of the OrpheeFile class containing all the data needed to create the MIDI file</param>
        /// <returns>Retuns a task containing a bool that is true if the file has been saved and false if it hasn't</returns>
        Task<string> SaveOrpheeFile(IOrpheeFile orpheeFile);
    }
}
