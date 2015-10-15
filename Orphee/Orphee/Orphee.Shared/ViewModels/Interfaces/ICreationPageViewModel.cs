using System.Collections.Generic;
using Windows.UI.Xaml.Media;
using Microsoft.Practices.Prism.Commands;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;

namespace Orphee.ViewModels.Interfaces
{
    public interface ICreationPageViewModel
    {
        // Methods
        void ToggleButtonNoteExec(IToggleButtonNote toggleButtonNote);
        // Properties
        int CurrentInstrumentIndex { get; set; }
        int CurrentTempoIndex { get; set; }
        List<uint> TempoValues { get; }
        IOrpheeFile OrpheeFile { get; }
        DelegateCommand AddColumnsCommand { get; }
        DelegateCommand RemoveAColumnCommand { get; }
        DelegateCommand<IToggleButtonNote> ToggleButtonNoteCommand { get; }
        DelegateCommand SaveButtonCommand { get; }
        DelegateCommand LoadButtonCommand { get; }
        DelegateCommand BackButtonCommand { get; }
        DelegateCommand<OrpheeTrack> SelectedTrackCommand { get; }
        DelegateCommand AddNewTrackCommand { get; }
        DelegateCommand PlayCommand { get; }
    }
}
