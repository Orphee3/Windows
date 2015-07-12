using Midi;
using Orphee.Models.Interfaces;

namespace Orphee.Models
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
    }
}
