using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;
using Orphee.ViewModels.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
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

        public Visibility IsTrackAdditionButtonVisible
        {
            get { return this._isTrackAdditionButtonVisible; }
            set
            {
                if (this._isTrackAdditionButtonVisible != value)
                    SetProperty(ref this._isTrackAdditionButtonVisible, value);
            }
        }
        private Visibility _isTrackAdditionButtonVisible = Visibility.Visible;
        public Visibility IsTrackDeletionButtonVisible
        {
            get { return this._isTrackDeletionButtonVisible; }
            set
            {
                if (this._isTrackDeletionButtonVisible != value)
                    SetProperty(ref this._isTrackDeletionButtonVisible, value);
            }
        }
        private Visibility _isTrackDeletionButtonVisible = Visibility.Visible;
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

        public bool IsHorizontalScrollingEnabled
        {
            get { return this._isHorizontalScrollingEnabled; }
            set
            {
                if (this._isHorizontalScrollingEnabled != value)
                    SetProperty(ref this._isHorizontalScrollingEnabled, value);
            }
        }

        private bool _isHorizontalScrollingEnabled = true;
        public int CurrentTrackPos
        {
            get { return this._currentTrackPos; }
            set
            {
                if (this._currentTrackPos != value)
                    SetProperty(ref this._currentTrackPos, value);
            }
        }

        private IToggleButtonNote _holdedRectangleButton;
        private bool _rectangleButtonHoldedStage;
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
        public DelegateCommand<IToggleButtonNote> ToggleButtonHolded { get; private set; }
        public DelegateCommand<SelectionChangedEventArgs> SelectedTrackCommand { get; private set; }
        public DelegateCommand AddOneHigherOctaveCommand { get; private set; }
        public DelegateCommand AddOneLowerOctaveCommand { get; private set; }
        public DelegateCommand ClearButtonCommand { get; private set; }
        public DelegateCommand<IToggleButtonNote> CursorEnteredToggleButtonCommand { get; private set; }
        public DelegateCommand DeleteTrackCommand { get; private set; }
        public DelegateCommand AddNewTrackCommand { get; private set; }
        public DelegateCommand LeaveCreationCommand { get; private set; }
        private int _invitedUserTrackPos;
        public INoteMapManager NoteMapManager { get; private set; }
        private IOrpheeTrack _selectedTrack;
        private bool? _creationMode;
        private readonly CoreDispatcher _dispatcher;

        public CreationPageViewModel(ISoundPlayer soundPlayer, IOrpheeFileManager orpheeFileManager, INoteMapManager noteMapManager, IOrpheeFile orpheeFile)
        {
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                this._dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            this.NoteMapManager = noteMapManager;
            this.OrpheeFile = orpheeFile.OrpheeTrackList.Count > 1 ? new OrpheeFile(new OrpheeTrack(new OrpheeTrackUI(new ColorManager()), new NoteMapManager())) : orpheeFile;
            this._currentChannel = Channel.Channel1;
            this._soundPlayer = soundPlayer;
            this.CurrentTrackPos = 0;
            InitTempo();
            this._orpheeFileManager = orpheeFileManager;
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Landscape;
            InitDelegateCommands();
         }

        private void InitTempo()
        {
            this.TempoValues = new List<uint>();
            for (var repeat = 0; repeat < 360; repeat++)
                this.TempoValues.Add((uint)(40 + repeat));
            this.CurrentTempoIndex = 80;
        }

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            if (RestApiManagerBase.Instance.IsConnected && App.InternetAvailabilityWatcher.IsInternetUp)
                InitMode();
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
            this.LeaveCreationCommand = new DelegateCommand(LeaveCreationCommandExec);
            //this.RemoveAColumnCommand = new DelegateCommand(() => this._noteMapManager.RemoveAColumnFromThisNoteMap(this.OrpheeFile.OrpheeTrackList.FirstOrDefault(t => t.Channel == this._currentChannel).NoteMap));
            this.SaveButtonCommand = new DelegateCommand(SaveButtonCommandExec);
            this.LoadButtonCommand = new DelegateCommand(LoadButtonCommandExec);
            this.BackButtonCommand = new DelegateCommand(() => { App.MyNavigationService.GoBack(); });
            this.PlayCommand = new DelegateCommand(PlayCommandExec);
            this.ToggleButtonHolded = new DelegateCommand<IToggleButtonNote>(ToggleButtonHoldedExec);
            this.CursorEnteredToggleButtonCommand = new DelegateCommand<IToggleButtonNote>(CursorEnteredToggleButtonCommandExec);
        }

        private async void InitMode()
        {
            var menuMessageDialog = new CreationPageMenuMessageDialog();
            var result1 = await App.InternetAvailabilityWatcher.SocketManager.SocketEmitter.SendGameRooms();
            await menuMessageDialog.ShowAsync();
            var result = menuMessageDialog.GetCreationType();
            if (result == null)
            {
                this._creationMode = null;
                return;
            }
            RestApiManagerBase.Instance.UserData.User.PropertyChanged += UserOnPropertyChanged;
            this._creationMode = result.Name == "+";
            if (this._creationMode == false)
            {
                DisableFunctionalities();
                var result2 = App.InternetAvailabilityWatcher.SocketManager.SocketEmitter.SendJoinGameRooms(result.Name, this.OrpheeFile.OrpheeTrackList.Last().CurrentInstrument);
            }
            else
            {
                var result2 = await App.InternetAvailabilityWatcher.SocketManager.SocketEmitter.SendCreateRoom();
            }
        }

        private void UserOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "_hasReceivedNewComerNotification" && RestApiManagerBase.Instance.UserData.User.HasReceivedNewComerNotification)
                AddNewComerToOrpheeFilesPeopleList();
            if (propertyChangedEventArgs.PropertyName == "_hasReceivedBigBangNotification" && RestApiManagerBase.Instance.UserData.User.HasReceivedBigBangNotification)
                ApplyBigBangEvent();
            if (propertyChangedEventArgs.PropertyName == "_hasReceivedKickNotification" && RestApiManagerBase.Instance.UserData.User.HasReceivedKickNotification)
                ApplyTheKick();
            if (propertyChangedEventArgs.PropertyName == "_hasReceivedLeavingNotification" && RestApiManagerBase.Instance.UserData.User.HasReceivedLeavingNotification)
                RemoveUsersTrackFromTrackList();
        }

        private async void RemoveUsersTrackFromTrackList()
        {
            if (RestApiManagerBase.Instance.UserData.User.LeavingUser == "")
                return;
            await this._dispatcher.RunAsync(CoreDispatcherPriority.Normal, RemoveUsersTrack);
        }

        private void RemoveUsersTrack()
        {
            this.OrpheeFile.OrpheeTrackList.Remove(this.OrpheeFile.OrpheeTrackList.FirstOrDefault(t => t.OwnerId == RestApiManagerBase.Instance.UserData.User.LeavingUser));
            RestApiManagerBase.Instance.UserData.User.HasReceivedLeavingNotification = false;
            RestApiManagerBase.Instance.UserData.User.LeavingUser = "";
        }

        private async void ApplyTheKick()
        {
            await this._dispatcher.RunAsync(CoreDispatcherPriority.Normal, ClearAndQuitPage);
            RestApiManagerBase.Instance.UserData.User.HasReceivedKickNotification = false;
        }

        private async void ApplyBigBangEvent()
        {
            if (this._creationMode != false)
                return;
            await this._dispatcher.RunAsync(CoreDispatcherPriority.Normal, ClearAndQuitPage);
            RestApiManagerBase.Instance.UserData.User.HasReceivedBigBangNotification = false;
        }

        private async void AddNewComerToOrpheeFilesPeopleList()
        {
            if (RestApiManagerBase.Instance.UserData.User.NewComer == "")
                return;
            this.OrpheeFile.AddNewPeople(RestApiManagerBase.Instance.UserData.User.NewComer);
            await this._dispatcher.RunAsync(CoreDispatcherPriority.Normal, AddNewComerTrack);
        }

        private void DisableFunctionalities()
        {
            this.IsTempoModificationEnabled = false;
            this.IsTrackDeletionOrAdditionEnabled = false;
            this.IsTrackAdditionButtonVisible = Visibility.Collapsed;
            this.IsTrackDeletionButtonVisible = Visibility.Collapsed;
            this._invitedUserTrackPos = this.OrpheeFile.OrpheeTrackList.Last().TrackPos;
        }

        private async void LeaveCreationCommandExec()
        {
            ClearAndQuitPage();
            if (this._creationMode != null)
            {
                var result = this._creationMode == true ? await App.InternetAvailabilityWatcher.SocketManager.SocketEmitter.SendPieceQuit() : await App.InternetAvailabilityWatcher.SocketManager.SocketEmitter.NewComerLeavesPiece();
                return;
            }
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
            foreach (var track in this.OrpheeFile.OrpheeTrackList)
            {
                track.ColumnMap.Clear();
                track.NoteMap.Clear();
            }
            this.OrpheeFile.OrpheeTrackList.Clear();
            var newTrack = new OrpheeTrack(new OrpheeTrackUI(new ColorManager()), new NoteMapManager());
            newTrack.Init(0, 0, true);
            UpdateCurrentInstrument(Instrument.AcousticGrandPiano);
            this.OrpheeFile.AddNewTrack(newTrack);
            this._currentChannel = Channel.Channel1;
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

        private void AddNewComerTrack()
        {
            if (this.OrpheeFile.OrpheeTrackList.Count >= 16)
                return;
            var newTrack = new OrpheeTrack(new OrpheeTrackUI(new ColorManager()), new NoteMapManager()) {OwnerId = RestApiManagerBase.Instance.UserData.User.NewComer };
            newTrack.Init(this.OrpheeFile.OrpheeTrackList.Count, (Channel)this.OrpheeFile.OrpheeTrackList.Count, true);
            this.OrpheeFile.AddNewTrack(newTrack);
            this.OrpheeFile.OrpheeFileParameters.NumberOfTracks++;
            RestApiManagerBase.Instance.UserData.User.NewComer = "";
            RestApiManagerBase.Instance.UserData.User.HasReceivedNewComerNotification = false;
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
                while (await App.InternetAvailabilityWatcher.SocketManager.SocketEmitter.SendData<string>("Columns", null))
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
            if (this._creationMode == false && selectedTrack.TrackPos != this._invitedUserTrackPos)
                return;
            var messageDialog = new MyMessageDialog();

            messageDialog.SetSource(selectedTrack);
            await messageDialog.ShowAsync();
            UpdateCurrentInstrument(selectedTrack.CurrentInstrument);
        }

        private void ToggleButtonHoldedExec(IToggleButtonNote holdedRectangleButton)
        {
            if (!this._rectangleButtonHoldedStage)
            {
                this._holdedRectangleButton = holdedRectangleButton;
                this._isHorizontalScrollingEnabled = false;
                this._rectangleButtonHoldedStage = true;
                return;
            }
            this._rectangleButtonHoldedStage = false;
            this._isHorizontalScrollingEnabled = true;
            return;
        }

        private void CursorEnteredToggleButtonCommandExec(IToggleButtonNote enteredToggleButton)
        {
            if (this._rectangleButtonHoldedStage)
                //this.OrpheeFile.OrpheeTrackList[(int)this._currentChannel].ColumnMap[enteredToggleButton.ColumnIndex].IsRectangleVisible = 0;
                return;

        }

        private void ClearAndQuitPage()
        {
            ClearButtonCommandExec();
            App.MyNavigationService.GoBack();
        }

        private async void DisplayMessage(string message)
        {
            var messageDialog = new MessageDialog(message);

            await messageDialog.ShowAsync();
        }
    }
}
