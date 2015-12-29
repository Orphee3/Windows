using System;
using System.Collections.Generic;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;
using Orphee.ViewModels.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Midi;
using Orphee.Models.Interfaces;
using Orphee.RestApiManagement.Models;
using Orphee.UI;

namespace Orphee.ViewModels
{
    public class CreationPageViewModel : ViewModel, ICreationPageViewModel
    {
        private readonly ISoundPlayer _soundPlayer;
        private readonly IOrpheeFileManager _orpheeFileManager;
        private bool _isTempoModificationEnabled = true;
        public bool IsTempoModificationEnabled
        {
            get { return this._isTempoModificationEnabled; }
            set
            {
                if (this._isTempoModificationEnabled != value)
                    SetProperty(ref this._isTempoModificationEnabled, value);
            }
        }
        private bool _isTrackDeletionOrAdditionEnabled = true;
        public bool IsTrackDeletionOrAdditionEnabled
        {
            get { return this._isTrackDeletionOrAdditionEnabled; }
            set
            {
                if (this._isTrackDeletionOrAdditionEnabled != value)
                    SetProperty(ref this._isTrackDeletionOrAdditionEnabled, value);
            }
        }
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
        public IOrpheeFile OrpheeFile { get; private set; }
        public DelegateCommand GoToUpperOctaveCommand { get; private set; }
        public DelegateCommand GoToLowerOctaveCommand { get; private set; }
        public DelegateCommand AddColumnsCommand { get; private set; }
        public DelegateCommand BackButtonCommand { get; private set; }
        public DelegateCommand<IToggleButtonNote> ToggleButtonNoteCommand { get; private set; }
        public DelegateCommand RemoveAColumnCommand { get; private set; }
        public DelegateCommand<OrpheeTrack> HoldTrackCommand { get; set; }
        public DelegateCommand SaveButtonCommand { get; private set; }
        public DelegateCommand ItemSelectedCommand { get; private set; }
        public DelegateCommand LoadButtonCommand { get; private set; }
        public DelegateCommand PlayCommand { get; private set; }
        public DelegateCommand<SelectionChangedEventArgs> SelectedTrackCommand { get; private set; }
        public DelegateCommand AddOneHigherOctaveCommand { get; private set; }
        public DelegateCommand AddOneLowerOctaveCommand { get; private set; }
        public DelegateCommand ClearButtonCommand { get; private set; }
        public DelegateCommand DeleteTrackCommand { get; private set; }
        public DelegateCommand AddNewTrackCommand { get; private set; }

        public INoteMapManager NoteMapManager { get; private set; }
        private IOrpheeTrack _selectedTrack;
        private bool? _creationMode;

        public CreationPageViewModel(ISoundPlayer soundPlayer, IOrpheeFileManager orpheeFileManager, INoteMapManager noteMapManager, IOrpheeFile orpheeFile)
         {
            this.NoteMapManager = noteMapManager;
            this.OrpheeFile = orpheeFile.OrpheeTrackList.Count > 1 ? new OrpheeFile(new OrpheeTrack(new OrpheeTrackUI(new ColorManager()), new NoteMapManager())) : orpheeFile;
            this._currentChannel = Channel.Channel1;
            this._soundPlayer = soundPlayer;
            this.CurrentTrackPos = 0;
            InitTempo();
            this._orpheeFileManager = orpheeFileManager;
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Landscape;
            InitDelegateCommands();
            if (RestApiManagerBase.Instance.IsConnected && App.InternetAvailabilityWatcher.IsInternetUp) 
                InitMode();
         }

        private void InitTempo()
        {
            this.TempoValues = new List<uint>();
            for (var repeat = 0; repeat < 360; repeat++)
                this.TempoValues.Add((uint)(40 + repeat));
            this.CurrentTempoIndex = 80;
        }

        private void InitDelegateCommands()
        {
            this.GoToUpperOctaveCommand = new DelegateCommand(GoToUpperOctaveCommandExec);
            this.GoToLowerOctaveCommand = new DelegateCommand(GoToLowerOctaveCommandExec);
            this.HoldTrackCommand = new DelegateCommand<OrpheeTrack>(HoldTrackCommandExec);
            this.ClearButtonCommand = new DelegateCommand(ClearButtonCommandExec);
            this.DeleteTrackCommand = new DelegateCommand(DeleteTrackCommandExec);
            this.ToggleButtonNoteCommand = new DelegateCommand<IToggleButtonNote>(ToggleButtonNoteExec);
            this.AddNewTrackCommand = new DelegateCommand(AddNewTrackCommandExec);
            this.SelectedTrackCommand = new DelegateCommand<SelectionChangedEventArgs>(SelectedTrackCommandExec);
            this.AddColumnsCommand = new DelegateCommand(AddColumnsCommandExec);
            //this.RemoveAColumnCommand = new DelegateCommand(() => this._noteMapManager.RemoveAColumnFromThisNoteMap(this.OrpheeFile.OrpheeTrackList.FirstOrDefault(t => t.Channel == this._currentChannel).NoteMap));
            this.SaveButtonCommand = new DelegateCommand(SaveButtonCommandExec);
            this.LoadButtonCommand = new DelegateCommand(LoadButtonCommandExec);
            this.BackButtonCommand = new DelegateCommand(() => { App.MyNavigationService.GoBack(); });
            this.PlayCommand = new DelegateCommand(PlayCommandExec);
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
            if (this._creationMode == false)
                DisableFunctionalities();
        }

        private void DisableFunctionalities()
        {
            this.IsTempoModificationEnabled = false;
            this.IsTrackDeletionOrAdditionEnabled = false;
        }

        public async void ToggleButtonNoteExec(IToggleButtonNote toggleButtonNote)
        {
            toggleButtonNote.IsChecked = toggleButtonNote.IsChecked > 0 ? 0 : 100;
            if (toggleButtonNote.IsChecked > 0)
            {
                this._soundPlayer.PlayNote(toggleButtonNote.Note, this._currentChannel);
                if (this.OrpheeFile.OrpheeTrackList[(int)this._currentChannel].ColumnMap[toggleButtonNote.ColumnIndex].IsRectangleVisible == 0)
                    this.OrpheeFile.OrpheeTrackList[(int)this._currentChannel].ColumnMap[toggleButtonNote.ColumnIndex].IsRectangleVisible = 100;
            }
            else if (this.NoteMapManager.IsColumnEmpty(toggleButtonNote.ColumnIndex, this.OrpheeFile.OrpheeTrackList[(int) this._currentChannel].NoteMap))
                this.OrpheeFile.OrpheeTrackList[(int) this._currentChannel].ColumnMap[toggleButtonNote.ColumnIndex].IsRectangleVisible = 0;
            if (this._creationMode != null && App.InternetAvailabilityWatcher.IsInternetUp && RestApiManagerBase.Instance.IsConnected)
                while (!await App.InternetAvailabilityWatcher.SocketManager.SocketEmitter.SendNote(toggleButtonNote, toggleButtonNote.IsChecked > 0, this._currentChannel, this.OrpheeFile.OrpheeTrackList.FirstOrDefault(t => t.Channel == this._currentChannel).CurrentOctaveIndex))
                    await Task.Delay(1);
        }

        private void ClearButtonCommandExec()
        {

        }

        private void DeleteTrackCommandExec()
        {

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
            if (!App.InternetAvailabilityWatcher.IsInternetUp)
                return;
            this.OrpheeFile.OrpheeTrackList[0].PlayerParameters = this._soundPlayer.GetPlayerParameters();
            var result = await this._orpheeFileManager.ExportOrpheeFile(this.OrpheeFile);
            if (result == "")
                return;
            DisplayMessage(result);
            this.OrpheeFile.HasBeenSent = true;
        }

        private async void LoadButtonCommandExec()
        {
            var importedOrpheeFile = await this._orpheeFileManager.ImportOrpheeFile();

            if (importedOrpheeFile == null)
            {
                DisplayMessage("An error has occured during import");
                return;
            }
            this._orpheeFileManager.InitOrpheeFileWithImportedOrpheeFile(importedOrpheeFile, this.OrpheeFile);
            InitFirstTrack();
        }

        private void InitFirstTrack()
        {
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
            UpdateProperties();
            ResetTracksVisibility();
        }

        private void UpdateProperties()
        {
            this.CurrentTrackPos = this._selectedTrack.TrackPos;
            this._currentChannel = this._selectedTrack.Channel;
            UpdateCurrentInstrument(this._selectedTrack.CurrentInstrument);
        }

        private void ResetTracksVisibility()
        {
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

        private async void AddColumnsCommandExec()
        {
            foreach (var track in this.OrpheeFile.OrpheeTrackList)
            {
                this.NoteMapManager.AddColumnsToThisNoteMap(track.NoteMap);
                this.NoteMapManager.AddColumnsToThisColumnMap(track.ColumnMap);
                while (await App.InternetAvailabilityWatcher.SocketManager.SocketEmitter.SendMoreColumns())
                    await Task.Delay(1);
            }
        }

        private void PlayCommandExec()
        {
            foreach (var track in this.OrpheeFile.OrpheeTrackList.Where(t => t.IsSolo))
            {
                if (track != null && track.ColumnMap != null && track.ColumnMap.Any(rect => rect.IsRectangleVisible != 0))
                {
                    track.ConvertNoteMapToOrpheeMessage();
                    this._soundPlayer.PlayTrack(track.OrpheeNoteMessageList, track.CurrentInstrument, track.Channel);
                }
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
