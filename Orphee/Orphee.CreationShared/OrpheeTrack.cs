using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Midi;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    public class OrpheeTrack : IOrpheeTrack, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<ObservableCollection<IToggleButtonNote>> _noteMap;
        public ObservableCollection<ObservableCollection<IToggleButtonNote>> NoteMap
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
        public IPlayerParameters PlayerParameters { get; set; }
        public IList<IOrpheeNoteMessage> OrpheeNoteMessageList { get; set; } 
        public Instrument CurrentInstrument { get; set; }
        public Channel Channel { get; set; }
        public List<SolidColorBrush> ColorBrushList { get; private set; }
        public SolidColorBrush TrackColor
        {
            get { return this._trackColor; }
            set
            {
                if (this._trackColor != value)
                {
                    this._trackColor = value;
                    OnPropertyChanged("TrackColor");
                }
            }
              
        }
        private SolidColorBrush _trackColor; 
        public Visibility TrackVisibility
        {
            get { return this._trackVisibility; }
            set
            {
                if (this._trackVisibility != value)
                {
                    this._trackVisibility = value;
                    OnPropertyChanged("TrackVisibility");
                }
            }
        }
        private Visibility _trackVisibility;
        private int _trackColorIndex;
        public int TrackColorIndex
        {
            get { return this._trackColorIndex; }
            set
            {
                if (this._trackColorIndex != value)
                {
                    this._trackColorIndex = value;
                    OnPropertyChanged("TrackColorIndex");
                    this.TrackColor = this.ColorBrushList[this.TrackColorIndex];
                }
            }
        }
        public int TrackPos { get; set; }
        public uint TrackLength { get; set; }
        private string _trackName;

        public bool IsChecked
        {
            get { return this._isChecked; }
            set
            {
                if (this._isChecked != value)
                {
                    this._isChecked = value;
                    OnPropertyChanged("IsChecked");
                }
            }
        }
        private bool _isChecked;

        public string TrackName
        {
            get { return this._trackName; }
            set
            {
                if (this._trackName != value)
                {
                    this._trackName = value;
                    OnPropertyChanged("TrackName");
                }
            }
        }

        public OrpheeTrack(int trackPos, Channel channel, bool isNewTrack)
        {
            this.TrackColorIndex = 0;
            this.ColorBrushList = new List<SolidColorBrush>();
            //var colorList = typeof (Colors).GetTypeInfo().DeclaredProperties.OfType<Color>();
            //this.ColorBrushList.Add(new SolidColorBrush(Color.FromArgb(0, 120, 199, 249)));
            //foreach (var color in colorList)
            //    this.ColorBrushList.Add(new SolidColorBrush(color));
            this.TrackName = (trackPos + 1) + ". " + this.CurrentInstrument.Name();
            this.NoteMap = isNewTrack ? NoteMapManager.Instance.GenerateNoteMap(4) : null;
            this.Channel = channel;
            this.TrackPos = trackPos;
            this.PlayerParameters = this.TrackPos == 0 ? new PlayerParameters() : null;
            this.TrackLength = (uint) (this.TrackPos == 0 ?  22 : 7);
        }

        public void UpdateCurrentInstrument(Instrument instrument)
        {
            this.CurrentInstrument = instrument;
            this.TrackName = this.CurrentInstrument.Name();
        }

        public void UpdateOrpheeTrack(IOrpheeTrack orpheeTrack)
        {
            this.NoteMap = NoteMapManager.Instance.ConvertOrpheeMessageListToNoteMap(orpheeTrack.OrpheeNoteMessageList);
            this.Channel = orpheeTrack.Channel;
            this.TrackPos = orpheeTrack.TrackPos;
            this.TrackName = this.CurrentInstrument.Name();
            UpdateCurrentInstrument(orpheeTrack.CurrentInstrument);
            this.PlayerParameters = orpheeTrack.PlayerParameters;
            this.TrackLength = TrackLength;
            this.IsChecked = this.TrackPos == 0;
            this.TrackVisibility = this.TrackPos == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public void ConvertNoteMapToOrpheeMessage()
        {
            var trackLength = this.TrackLength;
            this.OrpheeNoteMessageList = NoteMapManager.Instance.ConvertNoteMapToOrpheeNoteMessageList(this.NoteMap, (int)this.Channel, ref trackLength);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
