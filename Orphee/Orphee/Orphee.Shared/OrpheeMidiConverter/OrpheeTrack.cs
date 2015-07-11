using System.Collections.Generic;
using System.Collections.ObjectModel;
using Orphee.Models;
using Orphee.Models.Interfaces;
using Orphee.OrpheeMidiConverter.Interfaces;

namespace Orphee.OrpheeMidiConverter
{
    public class OrpheeTrack : IOrpheeTrack
    {
        public IList<ObservableCollection<IToggleButtonNote>> NoteMap { get; private set; }

        public OrpheeTrack()
        {
            this.NoteMap = NoteMapManager.Instance.GenerateNoteMap();
        }
    }
}
