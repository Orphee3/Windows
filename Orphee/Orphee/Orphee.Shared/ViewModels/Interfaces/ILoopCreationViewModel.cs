using Microsoft.Practices.Prism.Commands;
using Orphee.Models.Interfaces;

namespace Orphee.ViewModels.Interfaces
{
    public interface ILoopCreationViewModel
    {
        // Methods
        void ToggleButtonNoteExec(IToggleButtonNote toggleButtonNote);

        // Properties
        IOrpheeTrack DisplayedTrack { get; }
        DelegateCommand AddColumnsCommand { get; }
        DelegateCommand RemoveAColumnCommand { get; }
        DelegateCommand<IToggleButtonNote> ToggleButtonNoteCommand { get; }
    }
}
