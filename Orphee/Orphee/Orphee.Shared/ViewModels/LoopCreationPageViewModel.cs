using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Midi;
using MidiDotNet.ExportModule.Interfaces;
using MidiDotNet.ImportModule.Interfaces;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;
using Orphee.RestApiManagement;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class LoopCreationPageViewModel : ViewModel, ILoopCreationPageViewModel
    {
        public int CurrentInstrumentIndex
        {
            get { return this._currentInstrumentIndex; }
            set
            {
                base.SetProperty(ref this._currentInstrumentIndex, value);
                UpdateInstrumentManagerCurrentInstrument();
            }
        }
        private int _currentInstrumentIndex;
        private readonly ISoundPlayer _soundPlayer;
        private readonly IOrpheeFileExporter _orpheeFileExporter;
        private readonly IOrpheeFileImporter _orpheeFileImporter;
        public IInstrumentManager InstrumentManager { get; private set; }
        public IOrpheeTrack DisplayedTrack { get; private set; }
        public DelegateCommand AddColumnsCommand { get; private set; }
        public DelegateCommand BackButtonCommand { get; private set; }
        public DelegateCommand RemoveAColumnCommand { get; private set; }
        public DelegateCommand<IToggleButtonNote> ToggleButtonNoteCommand { get; private set; }
        public DelegateCommand SaveButtonCommand { get; private set; }
        public DelegateCommand LoadButtonCommand { get; private set; }
        public DelegateCommand PlayCommand { get; private set; }
        public string TrackName { get; set; }

        public LoopCreationPageViewModel(ISoundPlayer soundPlayer, IInstrumentManager instrumentManager, IOrpheeFileExporter orpheeFileExporter, IOrpheeFileImporter orpheeFileImporter)
        {
            this.DisplayedTrack = new OrpheeTrack(0, Channel.Channel1);
            this.TrackName = this.DisplayedTrack.TrackName;
            this._soundPlayer = soundPlayer;
            this.InstrumentManager = instrumentManager;
            this._orpheeFileExporter = orpheeFileExporter;
            this._orpheeFileImporter = orpheeFileImporter;
            this.DisplayedTrack.CurrentInstrument = this.InstrumentManager.CurrentInstrument;
            this.ToggleButtonNoteCommand = new DelegateCommand<IToggleButtonNote>(ToggleButtonNoteExec);
            this.AddColumnsCommand = new DelegateCommand(AddColumnsCommandExec);
            this.RemoveAColumnCommand = new DelegateCommand(RemoveAColumnCommandExec);
            this.SaveButtonCommand = new DelegateCommand(SaveButtonCommandExec);
            this.LoadButtonCommand = new DelegateCommand(LoadButtonCommandExec);
            this.BackButtonCommand = new DelegateCommand(() => {App.MyNavigationService.GoBack();});
            this.PlayCommand = new DelegateCommand(() =>
            {
                this.DisplayedTrack.ConvertNoteMapToOrpheeMessage();
                this._soundPlayer.PlayTrack(this.DisplayedTrack.OrpheeNoteMessageList);
            });
        }

        private void AddColumnsCommandExec()
        {
            NoteMapManager.Instance.AddColumnsToThisNoteMap(this.DisplayedTrack.NoteMap);
        }

        private void RemoveAColumnCommandExec()
        {
            NoteMapManager.Instance.RemoveAColumnFromThisNoteMap(this.DisplayedTrack.NoteMap);
        }

        public void ToggleButtonNoteExec(IToggleButtonNote toggleButtonNote)
        {
            if (toggleButtonNote.IsChecked)
                this._soundPlayer.PlayNote(toggleButtonNote.Note);
        }

        private void UpdateInstrumentManagerCurrentInstrument()
        {
            this.InstrumentManager.CurrentInstrument = this.InstrumentManager.InstrumentList[this.CurrentInstrumentIndex];
            this._soundPlayer.UpdatePlayingInstrument(this.InstrumentManager.CurrentInstrument);
            this.DisplayedTrack.CurrentInstrument = this.InstrumentManager.CurrentInstrument;
        }

        private void SaveButtonCommandExec()
        {
            if (!RestApiManagerBase.Instance.IsConnected)
            {
                App.MyNavigationService.Navigate("Login", null);
                return;
            }
            this.DisplayedTrack.PlayerParameters = this._soundPlayer.GetPlayerParameters();
            this._orpheeFileExporter.SaveOrpheeTrack(this.DisplayedTrack);
        }

        private async void LoadButtonCommandExec()
        {
            if (!RestApiManagerBase.Instance.IsConnected)
            {
                App.MyNavigationService.Navigate("Login", null);
                return;
            }
            var importedOrpheeFile = await this._orpheeFileImporter.ImportFile(".loop");

            if (importedOrpheeFile == null)
                return;
            var firstTrack = importedOrpheeFile.OrpheeTrackList[0];
            this.DisplayedTrack.UpdateOrpheeTrack(firstTrack);
            this.DisplayedTrack.CurrentInstrument = firstTrack.CurrentInstrument;
            this._soundPlayer.SetPlayerParameters(this.DisplayedTrack.PlayerParameters);
            this.CurrentInstrumentIndex = (int) this.DisplayedTrack.CurrentInstrument;
            this._soundPlayer.UpdatePlayingInstrument(this.DisplayedTrack.CurrentInstrument);
        }
    }
}
