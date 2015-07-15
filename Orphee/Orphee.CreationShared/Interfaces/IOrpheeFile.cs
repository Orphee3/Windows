using System.Collections.Generic;

namespace Orphee.CreationShared.Interfaces
{
    public interface IOrpheeFile
    {
        // Properties
        IList<IOrpheeTrack> OrpheeTrackList { get; set; }
        string FileName { get; set; }

        // Methods
        void AddNewTrack(IOrpheeTrack orpheeTrack);
        void ConvertTracksNoteMapToOrpheeNoteMessageList();
    }
}
