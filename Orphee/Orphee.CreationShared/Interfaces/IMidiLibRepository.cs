﻿using System.Collections.Generic;
using Midi;

namespace Orphee.CreationShared.Interfaces
{
    public interface IMidiLibRepository
    {
        // Properties
        IPlayerParameters PlayerParameters { get; set; }

        // Methods
        void PlayNote(Note note);
        void UpdatePlayingInstrument(Instrument newPlayingInstrument);
        void SetPlayerParameters(IPlayerParameters playerParameters);
        void PlayTrack(IList<IOrpheeNoteMessage> noteMessageList);
    }
}
