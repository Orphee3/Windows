﻿using Midi;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    public class SoundPlayer : ISoundPlayer
    {
        private readonly IMidiLibRepository _midiLibRepository;

        public SoundPlayer(IMidiLibRepository midiLibRepository)
        {
            this._midiLibRepository = midiLibRepository;
        }

        public void PlayNote(Note note)
        {
            this._midiLibRepository.PlayNote(note);
        }

        public void UpdatePlayingInstrument(Instrument newPlayingInstrument)
        {
            this._midiLibRepository.UpdatePlayingInstrument(newPlayingInstrument);
        }

        public IPlayerParameters GetPlayerParameters()
        {
            return this._midiLibRepository.PlayerParameters;
        }
    }
}
