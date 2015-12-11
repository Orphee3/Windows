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
using Windows.Graphics.Display;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
        /// <summary>Index of the current instrument </summary>
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

        private int _currentTrackPos;
        public int CurrentTrackPos
        {
            get { return this._currentTrackPos; }
            set
            {
                if (this._currentTrackPos != value)
                    SetProperty(ref this._currentTrackPos, value);
            }
        }
        /// <summary>OrpheeFile displayed at the screen </summary>
        public IOrpheeFile OrpheeFile { get; private set; }
        public DelegateCommand GoToUpperOctaveCommand { get; private set; }
        public DelegateCommand GoToLowerOctaveCommand { get; private set; }
        /// <summary>Adds columns to the current track's note map </summary>
        public DelegateCommand AddColumnsCommand { get; private set; }
        /// <summary>Redirects to the previous page </summary>
        public DelegateCommand BackButtonCommand { get; private set; }
        public DelegateCommand<IToggleButtonNote> ToggleButtonNoteCommand { get; private set; }
        /// <summary>Removes columns from the current track's note map </summary>
        public DelegateCommand RemoveAColumnCommand { get; private set; }
        public DelegateCommand<OrpheeTrack> HoldTrackCommand { get; set; }
        /// <summary>Saves the current OrpheeFile in a MIDI file </summary>
        public DelegateCommand SaveButtonCommand { get; private set; }
        public DelegateCommand ItemSelectedCommand { get; private set; }
        /// <summary>Loads a MIDI file</summary>
        public DelegateCommand LoadButtonCommand { get; private set; }
        /// <summary>Play the notes contained in each track of the OrpheeFile </summary>
        public DelegateCommand PlayCommand { get; private set; }
        /// <summary>Changes the track displayed </summary>
        public DelegateCommand<SelectionChangedEventArgs> SelectedTrackCommand { get; private set; }
        /// <summary>Add one higher octave to the actual track's note map </summary>
        public DelegateCommand AddOneHigherOctaveCommand { get; private set; }
        /// <summary>Adds one lower octave to the current track's note map </summary>
        public DelegateCommand AddOneLowerOctaveCommand { get; private set; }
        public DelegateCommand ClearButtonCommand { get; private set; }
        public DelegateCommand DeleteTrackCommand { get; private set; }
        /// <summary>Add a new track to the OrpheeFile </summary>
        public DelegateCommand AddNewTrackCommand { get; private set; }

        public INoteMapManager NoteMapManager { get; private set; }
        private IOrpheeTrack _selectedTrack;
        private bool? _creationMode;

        /// <summary>
        /// Constructor initializing soundPlayer, instrumentManager, orpheeFileExporter
        /// and orpheeFileImporter through dependency injection
        /// </summary>
        /// <param name="soundPlayer">Interface between the program and the MidiLibRepository class</param>
        /// <param name="instrumentManager">Instrument manager</param>
        /// <param name="orpheeFileExporter">Saves the OrpheeFile to a MIDI file</param>
        /// <param name="orpheeFileImporter">Imports a MIDI file and converts it to an OrpheeFile</param>
        public CreationPageViewModel(ISoundPlayer soundPlayer, IOrpheeFileExporter orpheeFileExporter, IOrpheeFileImporter orpheeFileImporter, INoteMapManager noteMapManager, IOrpheeFile orpheeFile)
         {
            this.NoteMapManager = noteMapManager;
            this.OrpheeFile = orpheeFile.OrpheeTrackList.Count > 1 ? new OrpheeFile(new OrpheeTrack(new OrpheeTrackUI(new ColorManager()), new NoteMapManager())) : orpheeFile;
            this._currentChannel = Channel.Channel1;
            this._soundPlayer = soundPlayer;
            this.CurrentTrackPos = 0;
            this.TempoValues = new List<uint>();
            for (var repeat = 0; repeat < 360; repeat++)
                this.TempoValues.Add((uint)(40 + repeat));
            this.CurrentTempoIndex = 80;
            this._orpheeFileExporter = orpheeFileExporter;
            this._orpheeFileImporter = orpheeFileImporter;
            this.GoToUpperOctaveCommand = new DelegateCommand(GoToUpperOctaveCommandExec);
            this.GoToLowerOctaveCommand = new DelegateCommand(GoToLowerOctaveCommandExec);
            this.HoldTrackCommand = new DelegateCommand<OrpheeTrack>(HoldTrackCommandExec);
            this.ClearButtonCommand = new DelegateCommand(ClearButtonCommandExec);
            this.DeleteTrackCommand = new DelegateCommand(DeleteTrackCommandExec);
            this.ToggleButtonNoteCommand = new DelegateCommand<IToggleButtonNote>(ToggleButtonNoteExec);
            //this.AddOneHigherOctaveCommand = new DelegateCommand(() => this._noteMapManager.AddOneHigherOctaveToThisNoteMap(this.OrpheeFile.OrpheeTrackList.FirstOrDefault(t => t.Channel == this._currentChannel).NoteMap));
            //this.AddOneLowerOctaveCommand = new DelegateCommand(() => this._noteMapManager.AddOneLowerOctaveToThisNoteMap(this.OrpheeFile.OrpheeTrackList.FirstOrDefault(t => t.Channel == this._currentChannel).NoteMap));
            this.AddNewTrackCommand = new DelegateCommand(AddNewTrackCommandExec);
            this.SelectedTrackCommand = new DelegateCommand<SelectionChangedEventArgs>(SelectedTrackCommandExec);
            this.AddColumnsCommand = new DelegateCommand(() =>
            {
                foreach (var track in this.OrpheeFile.OrpheeTrackList)
                {
                    this.NoteMapManager.AddColumnsToThisNoteMap(track.NoteMap);
                    this.NoteMapManager.AddColumnsToThisColumnMap(track.ColumnMap);
                }
            });
            //this.RemoveAColumnCommand = new DelegateCommand(() => this._noteMapManager.RemoveAColumnFromThisNoteMap(this.OrpheeFile.OrpheeTrackList.FirstOrDefault(t => t.Channel == this._currentChannel).NoteMap));
            this.SaveButtonCommand = new DelegateCommand(SaveButtonCommandExec);
            this.LoadButtonCommand = new DelegateCommand(LoadButtonCommandExec);
            this.BackButtonCommand = new DelegateCommand(() => { App.MyNavigationService.GoBack(); });
            this.PlayCommand = new DelegateCommand(() =>
            {
                foreach (var track in this.OrpheeFile.OrpheeTrackList.Where(t => t.IsSolo))
                {
                    if (track != null && track.ColumnMap != null && track.ColumnMap.Any(rect => rect.IsRectangleVisible != 0))
                    {
                        track.ConvertNoteMapToOrpheeMessage();
                        this._soundPlayer.PlayTrack(track.OrpheeNoteMessageList, track.CurrentInstrument, track.Channel);
                    }
                }
            });
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Landscape;
            InitMode();
         }

        private async void InitMode()
        {
            var menuMessageDialog = new CreationPageMenuMessageDialog();

            await menuMessageDialog.ShowAsync();
            var result = menuMessageDialog.GetCreationType();
            if (result == null)
                this._creationMode = null;
            else
                this._creationMode = result.Name == "Create a new group";
        }

        /// <summary>
        /// Emits the sound associated with the given toggleButtonNote
        /// </summary>
        /// <param name="toggleButtonNote"></param>
        public void ToggleButtonNoteExec(IToggleButtonNote toggleButtonNote)
        {
            toggleButtonNote.IsChecked = toggleButtonNote.IsChecked > 0 ? 0 : 100;
            if (toggleButtonNote.IsChecked > 0)
            {
                this._soundPlayer.PlayNote(toggleButtonNote.Note, this._currentChannel);
                if (this.OrpheeFile.OrpheeTrackList[(int)this._currentChannel].ColumnMap[toggleButtonNote.ColumnIndex].IsRectangleVisible == 0)
                    this.OrpheeFile.OrpheeTrackList[(int)this._currentChannel].ColumnMap[toggleButtonNote.ColumnIndex].IsRectangleVisible = 100;
            }
            else if (this.NoteMapManager.IsColumnEmpty(toggleButtonNote.ColumnIndex, this.OrpheeFile.OrpheeTrackList[(int)this._currentChannel].NoteMap))
                this.OrpheeFile.OrpheeTrackList[(int)this._currentChannel].ColumnMap[toggleButtonNote.ColumnIndex].IsRectangleVisible = 0;
        }

        private void ClearButtonCommandExec()
        {
            var trackToRemove = new List<IOrpheeTrack>();
            foreach (var track in this.OrpheeFile.OrpheeTrackList)
            {
                if (track.TrackPos == 0)
                    track.Init(0, Channel.Channel1, true);
                else
                    trackToRemove.Add(track);
            }
            foreach (var deletedTrack in trackToRemove)
                this.OrpheeFile.OrpheeTrackList.Remove(deletedTrack);
            this.CurrentTrackPos = 0;
            this._currentChannel = 0;
        }

        private void DeleteTrackCommandExec()
        {
            //this.OrpheeFile.OrpheeTrackList.Remove(this._selectedTrack);
            //if (this._selectedTrack.TrackPos > this.Cu)
        }

        public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.None;
        }

        private void UpdateCurrentInstrument(Instrument newCurrentInstrument)
        {
            this._soundPlayer.UpdateCurrentInstrument(newCurrentInstrument, this._currentChannel);
        }

        private void UpdateTempoValue()
        {
            var newTempo = this.TempoValues[this.CurrentTempoIndex];
            this.OrpheeFile.OrpheeTrackList[0].PlayerParameters.Tempo = newTempo;
            var actualTrack = this.OrpheeFile.OrpheeTrackList.FirstOrDefault(t => t.Channel == this._currentChannel);
            this._soundPlayer.UpdateTempo(newTempo, actualTrack.CurrentInstrument, actualTrack.Channel);
        }

        private async void SaveButtonCommandExec()
        {
            this.OrpheeFile.OrpheeTrackList[0].PlayerParameters = this._soundPlayer.GetPlayerParameters();
            if (!App.InternetAvailabilityWatcher.IsInternetUp)
                return;
            var result = await this._orpheeFileExporter.SaveOrpheeFile(this.OrpheeFile);
            if (result == "")
                return;
            DisplayMessage(result);
            this.OrpheeFile.HasBeenSent = true;
        }

        private async void LoadButtonCommandExec()
        {
            var importedOrpheeFile = await this._orpheeFileImporter.ImportFile(".mid");

            if (importedOrpheeFile == null)
            {
                DisplayMessage("An error has occured during import");
                return;
            }
            this.OrpheeFile.OrpheeTrackList.Clear();
            this.OrpheeFile.OrpheeFileParameters = importedOrpheeFile.OrpheeFileParameters;
            foreach (var track in importedOrpheeFile.OrpheeTrackList)
            {
                var newTrack = new OrpheeTrack(new OrpheeTrackUI(new ColorManager()), new NoteMapManager());
                newTrack.Init(track);
                this.OrpheeFile.AddNewTrack(newTrack);
            }
            this.OrpheeFile.FileName = importedOrpheeFile.FileName;
            var firstTrack = this.OrpheeFile.OrpheeTrackList[0];
            UpdateCurrentInstrument(firstTrack.CurrentInstrument);
            this.CurrentTempoIndex = (int)firstTrack.PlayerParameters.Tempo - 40;
            this._currentChannel = firstTrack.Channel;
            this.CurrentTrackPos = 0;
        }

        private void SelectedTrackCommandExec(SelectionChangedEventArgs viewData)
        {
            if (viewData.AddedItems?.Count == 0)
                return;
            this._selectedTrack = viewData.AddedItems[0] as OrpheeTrack;
            if (this._currentChannel == this._selectedTrack.Channel)
                return;
            this.CurrentTrackPos = this._selectedTrack.TrackPos;
            this._currentChannel = this._selectedTrack.Channel;
            UpdateCurrentInstrument(this._selectedTrack.CurrentInstrument);
            foreach (var track in this.OrpheeFile.OrpheeTrackList.Where(t => t != this._selectedTrack))
            {
                track.SetTrackVisibility(Visibility.Collapsed);
                track.IsChecked = false;
            }
            this.OrpheeFile.OrpheeTrackList.FirstOrDefault(t => t.Channel == this._currentChannel).SetTrackVisibility(Visibility.Visible);
        }

        private void AddNewTrackCommandExec()
        {
            if (this.OrpheeFile.OrpheeTrackList.Count < 16)
            {
                var newTrack = new OrpheeTrack(new OrpheeTrackUI(new ColorManager()), new NoteMapManager());
                newTrack.Init(this.OrpheeFile.OrpheeTrackList.Count, (Channel)this.OrpheeFile.OrpheeTrackList.Count, true);
                this.OrpheeFile.AddNewTrack(newTrack);
                this.OrpheeFile.OrpheeFileParameters.NumberOfTracks++;
            }
        }

        private void GoToUpperOctaveCommandExec()
        {
            if (this.OrpheeFile.OrpheeTrackList.FirstOrDefault(t => t.Channel == this._currentChannel).CurrentOctaveIndex == 0)
                return;
            this.OrpheeFile.OrpheeTrackList.FirstOrDefault(t => t.Channel == this._currentChannel).CurrentOctaveIndex--;
        }

        private void GoToLowerOctaveCommandExec()
        {
            if (this.OrpheeFile.OrpheeTrackList.FirstOrDefault(t => t.Channel == this._currentChannel).CurrentOctaveIndex == 7)
                return;
            this.OrpheeFile.OrpheeTrackList.FirstOrDefault(t => t.Channel == this._currentChannel).CurrentOctaveIndex++;
        }

        private async void HoldTrackCommandExec(OrpheeTrack selectedTrack)
        {
            var messageDialog = new MyMessageDialog();

            messageDialog.SetSource(selectedTrack);
            await messageDialog.ShowAsync();
            UpdateCurrentInstrument(selectedTrack.CurrentInstrument);
        }

        private async void DisplayMessage(string message)
        {
            var messageDialog = new MessageDialog(message);

            await messageDialog.ShowAsync();
        }
    }
}
