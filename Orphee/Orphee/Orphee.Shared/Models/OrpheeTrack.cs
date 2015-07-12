using System.Collections.Generic;
using System.Collections.ObjectModel;
using Midi;
using Orphee.Models.Interfaces;

namespace Orphee.Models
{
    public class OrpheeTrack : IOrpheeTrack
    {
        public IList<ObservableCollection<IToggleButtonNote>> NoteMap { get; private set; }
        public Instrument CurrentInstrument { get; set; }

        public OrpheeTrack()
        {
            this.NoteMap = NoteMapManager.Instance.GenerateNoteMap();
        }
    }
}
