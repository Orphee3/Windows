using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Midi;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    public class OrpheeTrack : IOrpheeTrack, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private IList<ObservableCollection<IToggleButtonNote>> _noteMap;
        public IList<ObservableCollection<IToggleButtonNote>> NoteMap
        {
            get { return this._noteMap; }
            set
            {
                if (this._noteMap != value)
                {
                    this._noteMap = value;
                    OnPropertyChanged("NoteMap");
                }
            }
        }
        public IList<IOrpheeNoteMessage> OrpheeNoteMessageList { get; set; } 
        public Instrument CurrentInstrument { get; set; }
        public IPlayerParameters PlayerParameters { get; set; }
        public Channel Channel { get; set; }
        public int TrackPos { get; set; }
        public uint TrackLength { get; set; }
        public string TrackName { get; set; }

        public OrpheeTrack(int trackPos, Channel channel)
        {
            this.TrackName = "New Loop";
            this.NoteMap = NoteMapManager.Instance.GenerateNoteMap();
            this.Channel = channel;
            this.TrackPos = trackPos;
            if (this.TrackPos == 0)
            {
                this.PlayerParameters = new PlayerParameters();
                this.TrackLength = 22;
            }
            else
                this.TrackLength = 7;
        }

        public void UpdateOrpheeTrack(IOrpheeTrack orpheeTrack)
        {
            foreach (var line in this.NoteMap)
                line.Clear();
            this.NoteMap = NoteMapManager.Instance.ConvertOrpheeMessageListToNoteMap(orpheeTrack.OrpheeNoteMessageList);     
            this.Channel = orpheeTrack.Channel;
            this.TrackPos = orpheeTrack.TrackPos;
            this.PlayerParameters = orpheeTrack.PlayerParameters;
            this.TrackLength = TrackLength;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
