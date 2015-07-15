using System.Collections.Generic;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    public class OrpheeFile : IOrpheeFile
    {
        public IList<IOrpheeTrack> OrpheeTrackList { get; set; }
        public string FileName { get; set; }

        public OrpheeFile()
        {
            this.OrpheeTrackList = new List<IOrpheeTrack>();
        }

        public void AddNewTrack(IOrpheeTrack orpheeTrack)
        {
            this.OrpheeTrackList.Add(orpheeTrack);
        }

        public void ConvertTracksNoteMapToOrpheeNoteMessageList()
        {
            foreach (var track in this.OrpheeTrackList)
                track.OrpheeNoteMessageList = NoteMapManager.Instance.ConvertNoteMapToOrpheeMessageList(track.NoteMap, (int)track.Channel);
        }
    }
}
