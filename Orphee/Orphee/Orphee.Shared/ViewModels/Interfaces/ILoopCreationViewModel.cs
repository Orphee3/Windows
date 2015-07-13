﻿using Microsoft.Practices.Prism.Commands;
using Orphee.CreationShared.Interfaces;

namespace Orphee.ViewModels.Interfaces
{
    public interface ILoopCreationViewModel
    {
        // Methods
        void ToggleButtonNoteExec(IToggleButtonNote toggleButtonNote);

        // Properties
        IInstrumentManager InstrumentManager { get; }
        int CurrentInstrumentIndex { get; set; }
        IOrpheeTrack DisplayedTrack { get; }
        DelegateCommand AddColumnsCommand { get; }
        DelegateCommand RemoveAColumnCommand { get; }
        DelegateCommand<IToggleButtonNote> ToggleButtonNoteCommand { get; }
        DelegateCommand SaveButtonCommand { get; }
        DelegateCommand LoadButtonCommand { get; }
    }
}
