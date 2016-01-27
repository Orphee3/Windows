using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Windows.UI.Xaml;
using Newtonsoft.Json;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    /// <summary>
    /// Class representing the data needed to create a MIDI file
    /// </summary>
    [DataContract]
    public class OrpheeFile : IOrpheeFile
    {
        [DataMember]
        public string Id { get; set; }
        /// <summary>PropertyChanged event handler </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>Parameters of the orphee file </summary>
        [DataMember]
        public IOrpheeFileParameters OrpheeFileParameters { get; set; }
        private ObservableCollection<IOrpheeTrack> _orpheeTrackList;
        /// <summary>List of track contained in the current file </summary>
        [DataMember]
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
        public bool HasBeenSent { get; set; }
        private string _fileName;
        /// <summary>Name of the current file </summary>
        [DataMember]
        public string FileName
        {
            get { return this._fileName; }
            set
            {
                if (this._fileName != value)
                {
                    this._fileName = value;
                    OnPropertyChanged("FileName");
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OrpheeFile()
        {
            
        }
        public OrpheeFile(IOrpheeTrack orpheeTrack)
        {
            orpheeTrack.Init(0, 0, true);
            this.OrpheeTrackList = new ObservableCollection<IOrpheeTrack>() {orpheeTrack};
            this.OrpheeFileParameters = new OrpheeFileParameters();
            this.FileName = "New Piece";
        }

        /// <summary>
        /// Adds a new track to the OrpheeTrackList
        /// </summary>
        /// <param name="orpheeTrack">OrpheeTrack to add</param>
        public void AddNewTrack(IOrpheeTrack orpheeTrack)
        {
            var howMany = OrpheeTrackList.Count(t => t.TrackName == orpheeTrack.TrackName);
            if (howMany != 0)
                orpheeTrack.TrackName += howMany;
            this.OrpheeTrackList.Add(orpheeTrack);
        }

        /// <summary>
        /// Handler of the OnPropertyChanged event
        /// </summary>
        /// <param name="propertyName">Name of the property that triggered the event</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
