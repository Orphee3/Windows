using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Midi;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    /// <summary>
    /// Representation of a MIDI track
    /// </summary>
    public class OrpheeTrack : IOrpheeTrack, INotifyPropertyChanged
    {
        /// <summary>PropertyChanged event handler </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<ObservableCollection<IToggleButtonNote>> _noteMap;
        /// <summary>Rectangle map represented on the CreationPage screen </summary>
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
        /// <summary>Player parameters </summary>
        public IPlayerParameters PlayerParameters { get; set; }
        /// <summary>List of noteMessage representation of the NoteMap </summary>
        public IList<IOrpheeNoteMessage> OrpheeNoteMessageList { get; set; }
        /// <summary>Current instrument </summary> 
        public Instrument CurrentInstrument { get; set; }
        /// <summary>Channel assigned to the track </summary>
        public Channel Channel { get; set; }
        public List<SolidColorBrush> ColorBrushList { get; private set; }
        /// <summary>Value representing the actual color associated to the track </summary>
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
        /// <summary>Represents the Visibility of the track on the CreationPage screen </summary>
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
        /// <summary>Value representing the color index of the track </summary>
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
        /// <summary>Graphical position of the track </summary>
        public int TrackPos { get; set; }
        /// <summary>Length of the track </summary>
        public uint TrackLength { get; set; }
        private string _trackName;
        /// <summary>Name of the track </summary>
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
        /// <summary>Represents the isCheck event of the rectangle associated to the toggleButtonNote</summary>
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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="trackPos">Track position of the actual track</param>
        /// <param name="channel">Channel related to the actual track</param>
        /// <param name="isNewTrack">Defines if the actual track is a new one or not</param>
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

        /// <summary>
        /// Sets the current instrument value
        /// </summary>
        /// <param name="instrument">Instrument value to set to the current instrument</param>
        public void UpdateCurrentInstrument(Instrument instrument)
        {
            this.CurrentInstrument = instrument;
            this.TrackName = this.CurrentInstrument.Name();
        }

        /// <summary>
        /// Copies the given track info to the current track
        /// </summary>
        /// <param name="orpheeTrack">OrpheeTrack to be copied</param>
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

        /// <summary>
        /// Converts the NoteMap variable to a list of NoteMessage
        /// </summary>
        public void ConvertNoteMapToOrpheeMessage()
        {
            var trackLength = this.TrackLength;
            this.OrpheeNoteMessageList = NoteMapManager.Instance.ConvertNoteMapToOrpheeNoteMessageList(this.NoteMap, (int)this.Channel, ref trackLength);
        }

        /// <summary>
        /// Handles the OnPropertyChanged event
        /// </summary>
        /// <param name="propertyName">Property name that triggered the event</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
