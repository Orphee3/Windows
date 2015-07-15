using System.ComponentModel;
using Microsoft.Practices.Prism.Commands;
using Midi;
using MidiDotNet.ExportModule;
using MidiDotNet.ExportModule.Interfaces;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;
using Orphee.LoopCreation;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class LoopCreationViewModel : ILoopCreationViewModel, INotifyPropertyChanged
    {
        public int CurrentInstrumentIndex
        {
            get { return this._currentInstrumentIndex; }
            set
            {
                if (this._currentInstrumentIndex != value)
                {
                    this._currentInstrumentIndex = value;
                    OnPropertyChanged("CurrentInstrumentIndex");
                    UpdateInstrumentManagerCurrentInstrument();
                }
            }
        }
        private int _currentInstrumentIndex;
        private readonly ISoundPlayer _soundPlayer;
        private readonly IOrpheeFileExporter _orpheeFileExporter;
        public IInstrumentManager InstrumentManager { get; private set; }
        public IOrpheeTrack DisplayedTrack { get; private set; }
        public DelegateCommand AddColumnsCommand { get; private set; }
        public DelegateCommand RemoveAColumnCommand { get; private set; }
        public DelegateCommand<IToggleButtonNote> ToggleButtonNoteCommand { get; private set; }
        public DelegateCommand SaveButtonCommand { get; private set; }
        public DelegateCommand LoadButtonCommand { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public LoopCreationViewModel(ISoundPlayer soundPlayer, IInstrumentManager instrumentManager, IOrpheeFileExporter orpheeFileExporter)
        {
            this.DisplayedTrack = new OrpheeTrack(0, Channel.Channel1);
            this._soundPlayer = soundPlayer;
            this.InstrumentManager = instrumentManager;
            this._orpheeFileExporter = orpheeFileExporter;
            this.DisplayedTrack.CurrentInstrument = this.InstrumentManager.CurrentInstrument;
            this.ToggleButtonNoteCommand = new DelegateCommand<IToggleButtonNote>(ToggleButtonNoteExec);
            this.AddColumnsCommand = new DelegateCommand(AddColumnsCommandExec);
            this.RemoveAColumnCommand = new DelegateCommand(RemoveAColumnCommandExec);
            this.SaveButtonCommand = new DelegateCommand(SaveButtonCommandExec);
            this.LoadButtonCommand = new DelegateCommand(LoadButtonCommandExec);
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
            if (!toggleButtonNote.IsChecked)
                this._soundPlayer.PlayNote(toggleButtonNote.Note);
            toggleButtonNote.IsChecked = !toggleButtonNote.IsChecked;
        }

        private void UpdateInstrumentManagerCurrentInstrument()
        {
            this.InstrumentManager.CurrentInstrument = this.InstrumentManager.InstrumentList[this.CurrentInstrumentIndex];
            this._soundPlayer.UpdatePlayingInstrument(this.InstrumentManager.CurrentInstrument);
            this.DisplayedTrack.CurrentInstrument = this.InstrumentManager.CurrentInstrument;
        }

        private void SaveButtonCommandExec()
        {
            this.DisplayedTrack.PlayerParameters = this._soundPlayer.GetPlayerParameters();
            this._orpheeFileExporter.SaveOrpheeTrack(this.DisplayedTrack);
        }

        private async void LoadButtonCommandExec()
        {
            var loadLoopFilePicker = new LoadLoopFilePicker();

            await loadLoopFilePicker.LoadLoop();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
