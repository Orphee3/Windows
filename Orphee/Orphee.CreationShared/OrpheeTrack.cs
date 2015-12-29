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
        private ObservableCollection<OctaveManager> _noteMap;
        /// <summary>Rectangle map represented on the CreationPage screen </summary>
        public ObservableCollection<OctaveManager> NoteMap
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
        public Instrument CurrentInstrument { get; set; } = Instrument.AcousticGrandPiano;
        /// <summary>Channel assigned to the track </summary>
        public Channel Channel { get; set; }
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

        private bool _isMuted;

        public bool IsMuted
        {
            get { return this._isMuted; }
            set
            {
                if (this._isMuted != value)
                {
                    this._isMuted = value;
                    OnPropertyChanged("IsMuted");
                    if (value)
                        this.IsSolo = false;
                }
            }
        }
        private bool _isSolo;

        public bool IsSolo
        {
            get { return this._isSolo; }
            set
            {
                if (this._isSolo != value)
                {
                    this._isSolo = value;
                    OnPropertyChanged("IsSolo");
                    if (value)
                        this.IsMuted = false;
                }
            }
        }

        private int _currentOctaveIndex;

        public int CurrentOctaveIndex
        {
            get { return this._currentOctaveIndex; }
            set
            {
                if (this._currentOctaveIndex != value)
                {
                    this._currentOctaveIndex = value;
                    OnPropertyChanged(nameof(this.CurrentOctaveIndex));
                    ResetOctavesVisibilityValue();
                }
            }
        }

        public ObservableCollection<MyRectangle> ColumnMap { get; set; }
        public IOrpheeTrackUI UI { get; set; }
        private readonly INoteMapGenerator _noteMapGenerator;

        public OrpheeTrack(IOrpheeTrackUI orpheeTrachUi, INoteMapGenerator noteMapGenerator)
        {
            this.UI = orpheeTrachUi;
            this._noteMapGenerator = noteMapGenerator;
        }
        
        public void Init(int trackPos, Channel channel, bool isNewTrack)
        {            
            SetProperties(trackPos, channel);
            this.PlayerParameters = this.TrackPos == 0 ? new PlayerParameters() : null;
            this.NoteMap?.Clear();
            this.NoteMap = isNewTrack ? InitializeNoteMap() : null;
            this.ColumnMap?.Clear();
            this.ColumnMap = isNewTrack ? this._noteMapGenerator.GenerateColumnMap(this.NoteMap) : null;
        }

        public void Init(IOrpheeTrack orpheeTrack)
        {
            SetProperties(orpheeTrack.TrackPos, orpheeTrack.Channel);
            this.NoteMap = this._noteMapGenerator.ConvertOrpheeMessageListToNoteMap(orpheeTrack.OrpheeNoteMessageList);
            this._currentOctaveIndex = 4;
            this.ColumnMap = this._noteMapGenerator.GenerateColumnMap(this.NoteMap);
            UpdateCurrentInstrument(orpheeTrack.CurrentInstrument);
            this.PlayerParameters = orpheeTrack.PlayerParameters; 
        }

        private ObservableCollection<OctaveManager> InitializeNoteMap()
        {
            var noteMap = new ObservableCollection<OctaveManager>();
            for (var index = 0; index < 8; index++)
                noteMap.Add(this._noteMapGenerator.GenerateNoteMap(index));
            this._currentOctaveIndex = 4;
            return noteMap;
        }

        private void SetProperties(int trackPos, Channel channel)
        {
            this.IsSolo = true;
            this.TrackPos = trackPos;
            this.UI.InitProperties(trackPos);
            this.TrackLength = (uint)(this.TrackPos == 0 ? 22 : 7);
            this.TrackName = (this.TrackPos + 1) + ". " + this.CurrentInstrument.Name();
            this.Channel = channel;
            this.IsChecked = this.TrackPos == 0;
        }

        public void SetTrackVisibility(Visibility trackVisibility)
        {
            this.UI.TrackVisibility = trackVisibility;
        }

        public SolidColorBrush GetTrackColor()
        {
            return this.UI.TrackColor;
        }

        private void ResetOctavesVisibilityValue()
        {
            foreach (var octaveMap in this.NoteMap)
                octaveMap.OctaveManagerUI.OctaveVisibility = octaveMap.OctavePos == this.CurrentOctaveIndex ? Visibility.Visible : Visibility.Collapsed;
        }

        public void SetTrackColor(SolidColorBrush color)
        {
            this.UI.TrackColor = color;
        }

        /// <summary>
        /// Sets the current instrument value
        /// </summary>
        /// <param name="instrument">Instrument value to set to the current instrument</param>
        public void UpdateCurrentInstrument(Instrument instrument)
        {
            this.CurrentInstrument = instrument;
            this.TrackName = this.TrackPos + 1 + ". " +this.CurrentInstrument.Name();
        }

        /// <summary>
        /// Converts the NoteMap variable to a list of NoteMessage
        /// </summary>
        public void ConvertNoteMapToOrpheeMessage()
        {
            var trackLength = this.TrackLength;
            this.OrpheeNoteMessageList = this._noteMapGenerator.ConvertNoteMapToOrpheeNoteMessageList(this.NoteMap, (int)this.Channel, ref trackLength);
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
