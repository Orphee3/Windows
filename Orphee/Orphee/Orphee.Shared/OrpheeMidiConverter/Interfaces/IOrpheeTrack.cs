using System.Collections.Generic;
using System.Collections.ObjectModel;
using Orphee.Models.Interfaces;

namespace Orphee.OrpheeMidiConverter.Interfaces
{
    public interface IOrpheeTrack
    {
        // Methods

        // Properties
        IList<ObservableCollection<IToggleButtonNote>> NoteMap { get; } 
    }
}
