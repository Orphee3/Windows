using System.Collections.Generic;
using Midi;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    public class NoteNameListManager : INoteNameListManager
    {
        public Dictionary<string, Note> NoteNameList { get; private set; }

        public NoteNameListManager()
        {
            this.NoteNameList = new Dictionary<string, Note>()
            {
                {"B3", Note.B3},
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

        public int GetLineIndexFromNote(Note note)
        {
            var index = 0;

            foreach (var pair in this.NoteNameList)
            {
                if (pair.Value == note || index == 12)
                    return index;
                index++;
            }
            return index;
        }
    }
}
