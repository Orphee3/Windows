using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Orphee.Models.Interfaces
{
    public interface IOrpheeTrack
    {
        // Methods

        // Properties
        IList<ObservableCollection<IToggleButtonNote>> NoteMap { get; } 
    }
}
