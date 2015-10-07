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

        public void PlayNote(Note note, Channel channel)
        {
            this._midiLibRepository.PlayNote(note, channel);
        }

        public IPlayerParameters GetPlayerParameters()
        {
            return this._midiLibRepository.PlayerParameters;
        }

        public void UpdateCurrentInstrument(Instrument currentInstrument, Channel channel)
        {
            this._midiLibRepository.UpdatePlayingInstrument(channel, currentInstrument);
        }

        public void UpdateTempo(uint tempo)
        {
            this._midiLibRepository.UpdateTempo(tempo);
        }

        public void SetPlayerParameters(IPlayerParameters playerParameters)
        {
            this._midiLibRepository.SetPlayerParameters(playerParameters);
        }

        public void PlayTrack(IList<IOrpheeNoteMessage> noteMessageList, Instrument instrument, Channel channel)
        {
            this._midiLibRepository.PlayTrack(noteMessageList, instrument, channel);
        }
    }
}
