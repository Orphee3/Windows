using System.Collections.Generic;
using Midi;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    /// <summary>
    /// Class containing all the octave information
    /// </summary>
    public class NoteNameListManager : INoteNameListManager
    {
        /// <summary>Dictionary containing the notes from the octave 1 to 8 </summary>
        public Dictionary<string, Note> NoteNameList { get; private set; }
        public List<string> NoteName { get; private set; } 

        /// <summary>
        /// Constructor
        /// </summary>
        public NoteNameListManager()
        {
            this.NoteName = new List<string>()
            {
                "Do",
                "Do#",
                "Ré",
                "Ré#",
                "Mi",
                "Fa",
                "Fa#",
                "Sol",
                "Sol#",
                "La",
                "La#",
                "Si"
            };
            this.NoteNameList = new Dictionary<string, Note>()
            {
                {"C0", Note.C0},
                {"C#0", Note.CSharp0},
                {"D0", Note.D0},
                {"D#0", Note.DSharp0},
                {"E0", Note.E0},
                {"F0", Note.F0},
                {"F#0", Note.FSharp0},
                {"G0", Note.G0},
                {"G#0", Note.GSharp0},
                {"A0", Note.A0},
                {"A#0", Note.ASharp0},
                {"B0", Note.B0},
                {"C1", Note.C1},
                {"C#1", Note.CSharp1},
                {"D1", Note.D1},
                {"D#1", Note.DSharp1},
                {"E1", Note.E1},
                {"F1", Note.F1},
                {"F#1", Note.FSharp1},
                {"G1", Note.G1},
                {"G#1", Note.GSharp1},
                {"A1", Note.A1},
                {"A#1", Note.ASharp1},
                {"B1", Note.B1},
                {"C2", Note.C2},
                {"C#2", Note.CSharp2},
                {"D2", Note.D2},
                {"D#2", Note.DSharp2},
                {"E2", Note.E2},
                {"F2", Note.F2},
                {"F#2", Note.FSharp2},
                {"G2", Note.G2},
                {"G#2", Note.GSharp2},
                {"A2", Note.A2},
                {"A#2", Note.ASharp2},
                {"B2", Note.B2},
                {"C3", Note.C3},
                {"C#3", Note.CSharp3},
                {"D3", Note.D3},
                {"D#3", Note.DSharp3},
                {"E3", Note.E3},
                {"F3", Note.F3},
                {"F#3", Note.FSharp3},
                {"G3", Note.G3},
                {"G#3", Note.GSharp3},
                {"A3", Note.A3},
                {"A#3", Note.ASharp3},
                {"B3", Note.B3},
                {"C4", Note.C4},
                {"C#4", Note.CSharp4},
                {"D4", Note.D4},
                {"D#4", Note.DSharp4},
                {"E4", Note.E4},
                {"F4", Note.F4},
                {"F#4", Note.FSharp4},
                {"G4", Note.G4},
                {"G#4", Note.GSharp4},
                {"A4", Note.A4},
                {"A#4", Note.ASharp4},
                {"B4", Note.B4},
                {"C5", Note.C5},
                {"C#5", Note.CSharp5},
                {"D5", Note.D5},
                {"D#5", Note.DSharp5},
                {"E5", Note.E5},
                {"F5", Note.F5},
                {"F#5", Note.FSharp5},
                {"G5", Note.G5},
                {"G#5", Note.GSharp5},
                {"A5", Note.A5},
                {"A#5", Note.ASharp5},
                {"B5", Note.B5},
                {"C6", Note.C6},
                {"C#6", Note.CSharp6},
                {"D6", Note.D6},
                {"D#6", Note.DSharp6},
                {"E6", Note.E6},
                {"F6", Note.F6},
                {"F#6", Note.FSharp6},
                {"G6", Note.G6},
                {"G#6", Note.GSharp6},
                {"A6", Note.A6},
                {"A#6", Note.ASharp6},
                {"B6", Note.B6},
                {"C7", Note.C7},
                {"C#7", Note.CSharp7},
                {"D7", Note.D7},
                {"D#7", Note.DSharp7},
                {"E7", Note.E7},
                {"F7", Note.F7},
                {"F#7", Note.FSharp7},
                {"G7", Note.G7},
                {"G#7", Note.GSharp7},
                {"A7", Note.A7},
                {"A#7", Note.ASharp7},
                {"B7", Note.B7},
                {"C8", Note.C8},
                {"C#8", Note.CSharp8},
                {"D8", Note.D8},
                {"D#8", Note.DSharp8},
                {"E8", Note.E8},
                {"F8", Note.F8},
                {"F#8", Note.FSharp8},
                {"G8", Note.G8},
                {"G#8", Note.GSharp8},
                {"A8", Note.A8},
                {"A#8", Note.ASharp8},
                {"B8", Note.B8},
            };
        }

        /// <summary>
        /// Determines if the creation of a higher octave is possible
        /// </summary>
        /// <param name="note">Lower note of the actual octave</param>
        /// <returns>Returns -1 if it can't be done or the value of the octave to be created</returns>
        public int CanAddHigherOctave(Note note)
        {
            if ((int) note >= 108)
                return -1;
            return (int) note / 12;
        }

        /// <summary>
        /// Determines if the creation of a lower octave is possible
        /// </summary>
        /// <param name="note">Lower note of the actual octave</param>
        /// <returns>Returns -1 if it can't be done or the value of the octave to be created</returns>
        public int CanAddLowerOctave(Note note)
        {
            if ((int)note <= 12)
                return -1;
            return ((int)note / 12) - 2;
        }

        /// <summary>
        /// Gets the line index of the given note
        /// </summary>
        /// <param name="note">Note used to determine its line</param>
        /// <param name="lowerOctave">Lower octave index</param>
        /// <returns>Returns the line index of the given note</returns>
        public int GetNoteLineIndex(Note note, int lowerOctave)
        {
            var lowerNote = this.NoteNameList["C" + lowerOctave];
            return note - lowerNote;
        }
    }
}
