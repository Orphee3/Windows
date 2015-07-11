using System.Collections.Generic;
using System.Collections.ObjectModel;
using Orphee.Models.Interfaces;

namespace Orphee.Models
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
