using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    public class OrpheeFile : IOrpheeFile
    {
        public IOrpheeFileParameters OrpheeFileParameters { get; set; }
        public IPlayerParameters PlayerParameters { get; set; }
        public ObservableCollection<IOrpheeTrack> OrpheeTrackList { get; set; }
        public string FileName { get; set; }

        public OrpheeFile()
        {
            this.OrpheeTrackList = new ObservableCollection<IOrpheeTrack> {new OrpheeTrack(0, 0) {IsChecked = true, TrackVisibility = Visibility.Visible} };
            this.OrpheeFileParameters = new OrpheeFileParameters();
            this.PlayerParameters = new PlayerParameters();
        }

        public void AddNewTrack(IOrpheeTrack orpheeTrack)
        {
            var howMany = OrpheeTrackList.Count(t => t.TrackName == orpheeTrack.TrackName);
            if (howMany != 0)
                orpheeTrack.TrackName += howMany;
            this.OrpheeTrackList.Add(orpheeTrack);
        }

        public void UpdateOrpheeFileParameters()
        {
            this.OrpheeFileParameters.NumberOfTracks = (ushort) this.OrpheeTrackList.Count;
            this.OrpheeFileParameters.OrpheeFileType = (ushort) (this.OrpheeFileParameters.NumberOfTracks < 2 ? 0 : 1);
        }
    }
}
