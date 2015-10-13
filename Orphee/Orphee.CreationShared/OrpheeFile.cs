using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    public class OrpheeFile : IOrpheeFile
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public IOrpheeFileParameters OrpheeFileParameters { get; set; }
        private ObservableCollection<IOrpheeTrack> _orpheeTrackList;

        public ObservableCollection<IOrpheeTrack> OrpheeTrackList
        {
            get {return _orpheeTrackList;}
            set
            {
                if (this._orpheeTrackList != value)
                {
                    this._orpheeTrackList = value;
                    OnPropertyChanged("OrpheeTrackList");
                }
            }
        }
        public string FileName { get; set; }

        public OrpheeFile()
        {
            this.OrpheeTrackList = new ObservableCollection<IOrpheeTrack> {new OrpheeTrack(0, 0, true) {IsChecked = true, TrackVisibility = Visibility.Visible} };
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
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
