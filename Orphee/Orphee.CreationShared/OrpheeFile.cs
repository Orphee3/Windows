using System.Collections.Generic;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    public class OrpheeFile : IOrpheeFile
    {
        public IOrpheeFileParameters OrpheeFileParameters { get; set; }
        public IList<IOrpheeTrack> OrpheeTrackList { get; set; }
        public string FileName { get; set; }

        public OrpheeFile()
        {
            this.OrpheeTrackList = new List<IOrpheeTrack>();
            this.OrpheeFileParameters = new OrpheeFileParameters();
        }

        public void AddNewTrack(IOrpheeTrack orpheeTrack)
        {
            this.OrpheeTrackList.Add(orpheeTrack);
        }

        public void UpdateOrpheeFileParameters()
        {
            this.OrpheeFileParameters.NumberOfTracks = (ushort) this.OrpheeTrackList.Count;
            this.OrpheeFileParameters.OrpheeFileType = (ushort) (this.OrpheeFileParameters.NumberOfTracks < 2 ? 0 : 1);
        }
    }
}
