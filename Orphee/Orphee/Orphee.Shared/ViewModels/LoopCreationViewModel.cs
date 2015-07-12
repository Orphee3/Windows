using System.ComponentModel;
using Microsoft.Practices.Prism.Commands;
using Orphee.Models;
using Orphee.Models.Interfaces;
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
        public IInstrumentManager InstrumentManager { get; private set; }
        public IOrpheeTrack DisplayedTrack { get; private set; }
        public DelegateCommand AddColumnsCommand { get; private set; }
        public DelegateCommand RemoveAColumnCommand { get; private set; }
        public DelegateCommand<IToggleButtonNote> ToggleButtonNoteCommand { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public LoopCreationViewModel(IOrpheeTrack orpheeTrack, ISoundPlayer soundPlayer, IInstrumentManager instrumentManager)
        {
            this.DisplayedTrack = orpheeTrack;
            this._soundPlayer = soundPlayer;
            this.InstrumentManager = instrumentManager;
            this.ToggleButtonNoteCommand = new DelegateCommand<IToggleButtonNote>(ToggleButtonNoteExec);
            this.AddColumnsCommand = new DelegateCommand(AddColumnsCommandExec);
            this.RemoveAColumnCommand = new DelegateCommand(RemoveAColumnCommandExec);
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

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
