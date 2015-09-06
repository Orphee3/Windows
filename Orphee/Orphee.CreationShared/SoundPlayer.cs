using System.Collections.Generic;
using Midi;
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

        public void SetPlayerParameters(IPlayerParameters playerParameters)
        {
            this._midiLibRepository.SetPlayerParameters(playerParameters);
        }

        public void PlayTrack(IList<IOrpheeNoteMessage> noteMessageList)
        {
            this._midiLibRepository.PlayTrack(noteMessageList);
        }
    }
}
