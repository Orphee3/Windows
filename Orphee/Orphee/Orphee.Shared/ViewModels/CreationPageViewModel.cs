using System.Collections.Generic;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using MidiDotNet.ExportModule.Interfaces;
using MidiDotNet.ImportModule.Interfaces;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;
using Orphee.ViewModels.Interfaces;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Midi;
using Orphee.UI;

namespace Orphee.ViewModels
{
    /// <summary>
    /// CreationPage view model
    /// </summary>
    public class CreationPageViewModel : ViewModel, ICreationPageViewModel
    {
        private readonly ISoundPlayer _soundPlayer;
        private readonly IOrpheeFileExporter _orpheeFileExporter;
        private readonly IOrpheeFileImporter _orpheeFileImporter;
        /// <summary>Instrument manager </summary>
        public IInstrumentManager InstrumentManager { get; private set; }
        /// <summary>Index of the current instrument </summary>
        public int CurrentInstrumentIndex
        {
            get { return this._currentInstrumentIndex; }
            set
            {
                SetProperty(ref this._currentInstrumentIndex, value);
                UpdateCurrentInstrument();
            }
        }
        private int _currentInstrumentIndex;
        private Channel _currentChannel;
        /// <summary>List of int containing tempo values from 40 to 400 </summary>
        public List<uint> TempoValues { get; private set; }
        private int _currentTempoIndex;
        /// <summary>Index of the current tempo </summary>
        public int CurrentTempoIndex
        { 
            get { return this._currentTempoIndex; }
            set
            {
                if (this._currentTempoIndex != value)
                {
                    SetProperty(ref this._currentTempoIndex, value);
                    UpdateTempoValue();
                }
            }
        }
        /// <summary>OrpheeFile displayed at the screen </summary>
        public IOrpheeFile OrpheeFile { get; private set; }
        /// <summary>Adds columns to the current track's note map </summary>
        public DelegateCommand AddColumnsCommand { get; private set; }
        /// <summary>Redirects to the previous page </summary>
        public DelegateCommand BackButtonCommand { get; private set; }
        /// <summary>Removes columns from the current track's note map </summary>
        public DelegateCommand RemoveAColumnCommand { get; private set; }
        /// <summary>Calls the ToggleButtonNoteCommandExec </summary>
        public DelegateCommand<IToggleButtonNote> ToggleButtonNoteCommand { get; private set; }
        /// <summary>Saves the current OrpheeFile in a MIDI file </summary>
        public DelegateCommand SaveButtonCommand { get; private set; }
        public DelegateCommand ItemSelectedCommand { get; private set; }
        /// <summary>Loads a MIDI file</summary>
        public DelegateCommand LoadButtonCommand { get; private set; }
        /// <summary>Play the notes contained in each track of the OrpheeFile </summary>
        public DelegateCommand PlayCommand { get; private set; }
        /// <summary>Changes the track displayed </summary>
        public DelegateCommand<OrpheeTrack> SelectedTrackCommand { get; private set; }
        /// <summary>Add one higher octave to the actual track's note map </summary>
        public DelegateCommand AddOneHigherOctaveCommand { get; private set; }
        /// <summary>Adds one lower octave to the current track's note map </summary>
        public DelegateCommand AddOneLowerOctaveCommand { get; private set; }
        /// <summary>Add a new track to the OrpheeFile </summary>
        public DelegateCommand AddNewTrackCommand { get; private set; }
        public DelegateCommand<OrpheeTrack> TrackParametersCommand { get; private set; }

        /// <summary>
        /// Constructor initializing soundPlayer, instrumentManager, orpheeFileExporter
        /// and orpheeFileImporter through dependency injection
        /// </summary>
        /// <param name="soundPlayer">Interface between the program and the MidiLibRepository class</param>
        /// <param name="instrumentManager">Instrument manager</param>
        /// <param name="orpheeFileExporter">Saves the OrpheeFile to a MIDI file</param>
        /// <param name="orpheeFileImporter">Imports a MIDI file and converts it to an OrpheeFile</param>
        public CreationPageViewModel(ISoundPlayer soundPlayer, IInstrumentManager instrumentManager, IOrpheeFileExporter orpheeFileExporter, IOrpheeFileImporter orpheeFileImporter)
        {
            this.OrpheeFile = new OrpheeFile();
            this._currentChannel = Channel.Channel1;
            this._soundPlayer = soundPlayer;
            this.InstrumentManager = instrumentManager;
            this.TempoValues = new List<uint>();
            for (var repeat = 0; repeat < 360; repeat++)
                this.TempoValues.Add((uint)(40 + repeat));
            this.CurrentTempoIndex = 80;
            this._orpheeFileExporter = orpheeFileExporter;
            this._orpheeFileImporter = orpheeFileImporter;
            this.TrackParametersCommand = new DelegateCommand<OrpheeTrack>(TrackParametersCommandExec);
            this.AddOneHigherOctaveCommand = new DelegateCommand(() => NoteMapManager.Instance.AddOneHigherOctaveToThisNoteMap(this.OrpheeFile.OrpheeTrackList.FirstOrDefault(t => t.Channel == this._currentChannel).NoteMap));
            this.AddOneLowerOctaveCommand = new DelegateCommand(() => NoteMapManager.Instance.AddOneLowerOctaveToThisNoteMap(this.OrpheeFile.OrpheeTrackList.FirstOrDefault(t => t.Channel == this._currentChannel).NoteMap));
            this.AddNewTrackCommand = new DelegateCommand(AddNewTrackCommandExec);
            this.SelectedTrackCommand = new DelegateCommand<OrpheeTrack>(SelectedTrackCommandExec);
            this.ToggleButtonNoteCommand = new DelegateCommand<IToggleButtonNote>(ToggleButtonNoteExec);
            this.AddColumnsCommand = new DelegateCommand(() =>
            {
                foreach (var track in this.OrpheeFile.OrpheeTrackList)
                    NoteMapManager.Instance.AddColumnsToThisNoteMap(track.NoteMap);
            });
            this.RemoveAColumnCommand = new DelegateCommand(() => NoteMapManager.Instance.RemoveAColumnFromThisNoteMap(this.OrpheeFile.OrpheeTrackList.FirstOrDefault(t => t.Channel == this._currentChannel).NoteMap));
            this.SaveButtonCommand = new DelegateCommand(SaveButtonCommandExec);
            this.LoadButtonCommand = new DelegateCommand(LoadButtonCommandExec);
            this.BackButtonCommand = new DelegateCommand(() => {App.MyNavigationService.GoBack();});
            this.PlayCommand = new DelegateCommand(() =>
            {
                foreach (var track in this.OrpheeFile.OrpheeTrackList)
                {
                    track.ConvertNoteMapToOrpheeMessage();
                    this._soundPlayer.PlayTrack(track.OrpheeNoteMessageList, track.CurrentInstrument, track.Channel);
                }
            });
        }

        /// <summary>
        /// Emits the sound associated with the given toggleButtonNote
        /// </summary>
        /// <param name="toggleButtonNote"></param>
        public void ToggleButtonNoteExec(IToggleButtonNote toggleButtonNote)
        {
            if (toggleButtonNote.IsChecked)
            {
                this._soundPlayer.PlayNote(toggleButtonNote.Note, this._currentChannel);
                if (this.OrpheeFile.OrpheeTrackList[(int) this._currentChannel].ColumnMap[toggleButtonNote.ColumnIndex].RectangleBackgroundColor.Color == Colors.Gray)
                    this.OrpheeFile.OrpheeTrackList[(int)this._currentChannel].ColumnMap[toggleButtonNote.ColumnIndex].RectangleBackgroundColor = this.OrpheeFile.OrpheeTrackList[(int)this._currentChannel].TrackColor;
            }
            else if (NoteMapManager.Instance.IsColumnEmpty(toggleButtonNote.ColumnIndex, this.OrpheeFile.OrpheeTrackList[(int)this._currentChannel].NoteMap))
                this.OrpheeFile.OrpheeTrackList[(int)this._currentChannel].ColumnMap[toggleButtonNote.ColumnIndex].RectangleBackgroundColor =  new SolidColorBrush(Colors.Gray);
        }

        private void UpdateCurrentInstrument()
        {
            var newCurrentInstrument = this.InstrumentManager.InstrumentList[this.CurrentInstrumentIndex];
            this.InstrumentManager.CurrentInstrument = newCurrentInstrument;
            this.OrpheeFile.OrpheeTrackList.FirstOrDefault(t => t.Channel == this._currentChannel).UpdateCurrentInstrument(newCurrentInstrument);
            this.OrpheeFile.OrpheeTrackList.FirstOrDefault(t => t.Channel == this._currentChannel).TrackVisibility = Visibility.Visible;
            this._soundPlayer.UpdateCurrentInstrument(newCurrentInstrument, this._currentChannel);
        }

        private void UpdateTempoValue()
        {
            var newTempo = this.TempoValues[this.CurrentTempoIndex];
            this.OrpheeFile.OrpheeTrackList[0].PlayerParameters.Tempo = newTempo;
            this._soundPlayer.UpdateTempo(newTempo);
        }

        private async void SaveButtonCommandExec()
        {
            //if (!RestApiManagerBase.Instance.IsConnected)
            //{
            //    App.MyNavigationService.Navigate("Login", null);
            //        return;
            //}
            this.OrpheeFile.OrpheeTrackList[0].PlayerParameters = this._soundPlayer.GetPlayerParameters();
            var result = await this._orpheeFileExporter.SaveOrpheeFile(this.OrpheeFile);
        }

        private async void LoadButtonCommandExec()
        {
            //if (!RestApiManagerBase.Instance.IsConnected)
            //{
            //    App.MyNavigationService.Navigate("Login", null);
            //    return;
            //}
            var importedOrpheeFile = await this._orpheeFileImporter.ImportFile(".mid");

            if (importedOrpheeFile == null)
                return;
            for (var trackIndex = 0; trackIndex < importedOrpheeFile.OrpheeTrackList.Count; trackIndex++)
                 this.OrpheeFile.AddNewTrack(new OrpheeTrack(trackIndex, (Channel) trackIndex + 1, false));
            var firstTrack = this.OrpheeFile.OrpheeTrackList[0];
            this.CurrentTempoIndex = (int)firstTrack.PlayerParameters.Tempo - 40;
            this._currentChannel = firstTrack.Channel;
            this.CurrentInstrumentIndex = (int)firstTrack.CurrentInstrument;
        }

        private void SelectedTrackCommandExec(OrpheeTrack selectedTrack)
        {
            if (this._currentChannel != selectedTrack.Channel)
            {
                this._currentChannel = selectedTrack.Channel;
                this.CurrentInstrumentIndex = (int)selectedTrack.CurrentInstrument;
                foreach (var track in this.OrpheeFile.OrpheeTrackList.Where(t => t != selectedTrack))
                {
                    track.IsChecked = false;
                    track.TrackVisibility = Visibility.Collapsed;
                }
            }
        }

        private void AddNewTrackCommandExec()
        {
            if (this.OrpheeFile.OrpheeTrackList.Count < 16)
            {
                this.OrpheeFile.AddNewTrack(new OrpheeTrack(this.OrpheeFile.OrpheeTrackList.Count, (Channel) this.OrpheeFile.OrpheeTrackList.Count, true) {TrackVisibility = Visibility.Collapsed});
                this.OrpheeFile.OrpheeFileParameters.NumberOfTracks++;
                if (this.OrpheeFile.OrpheeTrackList.Count == 2)
                //To remove
                this.OrpheeFile.OrpheeTrackList[1].UpdateCurrentInstrument(Instrument.ChoirAahs);
                if (this.OrpheeFile.OrpheeTrackList.Count == 3)
                    //To remove
                    this.OrpheeFile.OrpheeTrackList[2].UpdateCurrentInstrument(Instrument.OverdrivenGuitar);
                if (this.OrpheeFile.OrpheeTrackList.Count == 4)
                    //To remove
                    this.OrpheeFile.OrpheeTrackList[3].UpdateCurrentInstrument(Instrument.OrchestralHarp);
            }
        }

        private async void TrackParametersCommandExec(OrpheeTrack selectedTrack)
        {
            var message = new TrackOptionsMessageDialog();
            await message.ShowAsync();
            // var dispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;
            // await dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await message.ShowAsync());
        }
    }
}
