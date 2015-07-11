using System.Collections.Generic;
using Midi;

namespace Orphee.Models
{
    public class NoteNameListManager
    {
        public Dictionary<string, Note> NoteNameList { get; private set; }

        public NoteNameListManager()
        {
            this.NoteNameList = new Dictionary<string, Note>()
            {
                {"C", Note.C4},
                {"C#", Note.CSharp4},
                {"D", Note.D4},
                {"D#", Note.DSharp4},
                {"E", Note.E4},
                {"F", Note.F4},
                {"F#", Note.FSharp4},
                {"G", Note.G4},
                {"G#", Note.GSharp4},
                {"A", Note.A4},
                {"A#", Note.ASharp4},
                {"B", Note.B4},
            };
        }
    }
}
