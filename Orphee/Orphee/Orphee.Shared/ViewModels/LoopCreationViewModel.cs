using Microsoft.Practices.Prism.Commands;
using Orphee.Models;
using Orphee.Models.Interfaces;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class LoopCreationViewModel : ILoopCreationViewModel
    {
        private readonly ISoundPlayer _soundplayer;
        public IOrpheeTrack DisplayedTrack { get; private set; }
        public DelegateCommand AddColumnsCommand { get; private set; }
        public DelegateCommand RemoveAColumnCommand { get; private set; }
        public DelegateCommand<IToggleButtonNote> ToggleButtonNoteCommand { get; private set; }

        public LoopCreationViewModel(IOrpheeTrack orpheeTrack, ISoundPlayer soundPlayer)
        {
            this.DisplayedTrack = orpheeTrack;
            this._soundplayer = soundPlayer;
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
                this._soundplayer.PlayNote(toggleButtonNote.Note);
            toggleButtonNote.IsChecked = !toggleButtonNote.IsChecked;
        }
    }
}
