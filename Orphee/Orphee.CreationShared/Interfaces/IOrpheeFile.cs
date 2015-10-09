using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Orphee.CreationShared.Interfaces
{
    public interface IOrpheeFile
    {
        // Properties
        IOrpheeFileParameters OrpheeFileParameters { get; set; }
        ObservableCollection<IOrpheeTrack> OrpheeTrackList { get; set; }
        string FileName { get; set; }

        // Methods
        void AddNewTrack(IOrpheeTrack orpheeTrack);
    }
}
