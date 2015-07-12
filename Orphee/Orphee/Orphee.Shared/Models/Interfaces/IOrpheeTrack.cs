﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using Midi;

namespace Orphee.Models.Interfaces
{
    public interface IOrpheeTrack
    {
        // Methods

        // Properties
        IList<ObservableCollection<IToggleButtonNote>> NoteMap { get; }
        Instrument CurrentInstrument { get; set; }
    }
}
