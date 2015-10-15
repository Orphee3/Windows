using System;
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

namespace Orphee.ViewModels
{
    public class CreationPageViewModel : ViewModel, ICreationPageViewModel
    {
        private readonly ISoundPlayer _soundPlayer;
        private readonly IOrpheeFileExporter _orpheeFileExporter;
        private readonly IOrpheeFileImporter _orpheeFileImporter;
        public IInstrumentManager InstrumentManager { get; private set; }
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
        public List<uint> TempoValues { get; private set; }
        private int _currentTempoIndex;
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
        public IOrpheeFile OrpheeFile { get; private set; }
        public DelegateCommand AddColumnsCommand { get; private set; }
        public DelegateCommand BackButtonCommand { get; private set; }
        public DelegateCommand RemoveAColumnCommand { get; private set; }
        public DelegateCommand<IToggleButtonNote> ToggleButtonNoteCommand { get; private set; }
        public DelegateCommand SaveButtonCommand { get; private set; }
        public DelegateCommand LoadButtonCommand { get; private set; }
        public DelegateCommand PlayCommand { get; private set; }
        public DelegateCommand<OrpheeTrack> SelectedTrackCommand { get; private set; }
        public DelegateCommand AddOneHigherOctaveCommand { get; private set; }
        public DelegateCommand AddOneLowerOctaveCommand { get; private set; }
        public DelegateCommand AddNewTrackCommand { get; private set; }

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

        public void ToggleButtonNoteExec(IToggleButtonNote toggleButtonNote)
        {
            if (toggleButtonNote.IsChecked)
                this._soundPlayer.PlayNote(toggleButtonNote.Note, this._currentChannel);
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
            {
                if (trackIndex >= this.OrpheeFile.OrpheeTrackList.Count)
                    this.OrpheeFile.AddNewTrack(new OrpheeTrack(trackIndex, (Channel) trackIndex + 1, false));
                this.OrpheeFile.OrpheeTrackList[trackIndex].UpdateOrpheeTrack(
                    importedOrpheeFile.OrpheeTrackList[trackIndex]);
            }
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
                this.CurrentInstrumentIndex = (int) selectedTrack.CurrentInstrument;
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
            }
        }
    }
}
