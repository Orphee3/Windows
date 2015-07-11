using Microsoft.Practices.Prism.Commands;
using Orphee.Models;
using Orphee.OrpheeMidiConverter.Interfaces;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class LoopCreationViewModel : ILoopCreationViewModel
    {
        public IOrpheeTrack DisplayedTrack { get; private set; }
        public DelegateCommand AddColumnsCommand { get; private set; }
        public DelegateCommand RemoveAColumnCommand { get; private set; }

        public LoopCreationViewModel(IOrpheeTrack orpheeTrack)
        {
            this.DisplayedTrack = orpheeTrack;
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
    }
}
