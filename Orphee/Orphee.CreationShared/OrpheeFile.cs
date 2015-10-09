using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    public class OrpheeFile : IOrpheeFile
    {
        public IOrpheeFileParameters OrpheeFileParameters { get; set; }
        public ObservableCollection<IOrpheeTrack> OrpheeTrackList { get; set; }
        public string FileName { get; set; }

        public OrpheeFile()
        {
            this.OrpheeTrackList = new ObservableCollection<IOrpheeTrack> {new OrpheeTrack(0, 0) {IsChecked = true, TrackVisibility = Visibility.Visible} };
            this.OrpheeFileParameters = new OrpheeFileParameters();
            this.FileName = "Test.mid";
        }

        public void AddNewTrack(IOrpheeTrack orpheeTrack)
        {
            var howMany = OrpheeTrackList.Count(t => t.TrackName == orpheeTrack.TrackName);
            if (howMany != 0)
                orpheeTrack.TrackName += howMany;
            this.OrpheeTrackList.Add(orpheeTrack);
        }

    }
}
