using System.Collections.Generic;
using Midi;

namespace Orphee.Models.Interfaces
{
    public interface INoteNameListManager
    {
        // Properties
        Dictionary<string, Note> NoteNameList { get; }

        // Methods
    }
}
