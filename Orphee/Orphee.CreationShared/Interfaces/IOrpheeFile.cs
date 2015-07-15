using System.Collections.Generic;

namespace Orphee.CreationShared.Interfaces
{
    public interface IOrpheeFile
    {
        // Properties
        IOrpheeFileParameters OrpheeFileParameters { get; set; }
        IList<IOrpheeTrack> OrpheeTrackList { get; set; }
        string FileName { get; set; }

        // Methods
        void AddNewTrack(IOrpheeTrack orpheeTrack);
        void UpdateOrpheeFileParameters();
    }
}
