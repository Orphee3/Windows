using System.Collections.Generic;
using Midi;

namespace Orphee.CreationShared.Interfaces
{
    /// <summary>
    /// NoteNameListManager interface
    /// </summary>
    public interface INoteNameListManager
    {
        // Properties

        /// <summary>Dictionary containing the notes from the octave 1 to 8 </summary>
        Dictionary<string, Note> NoteNameList { get; }

        // Methods

        /// <summary>
        /// Determines if the creation of a higher octave is possible
        /// </summary>
        /// <param name="note">Lower note of the actual octave</param>
        /// <returns>Returns -1 if it can't be done or the value of the octave to be created</returns>
        int CanAddHigherOctave(Note note);

        /// <summary>
        /// Determines if the creation of a lower octave is possible
        /// </summary>
        /// <param name="note">Lower note of the actual octave</param>
        /// <returns>Returns -1 if it can't be done or the value of the octave to be created</returns>
        int CanAddLowerOctave(Note note);

        /// <summary>
        /// Gets the line index of the given note
        /// </summary>
        /// <param name="note">Note used to determine its line</param>
        /// <param name="lowerOctave">Lower octave index</param>
        /// <returns>Returns the line index of the given note</returns>
        int GetNoteLineIndex(Note note, int lowerOctave);
    }
}
