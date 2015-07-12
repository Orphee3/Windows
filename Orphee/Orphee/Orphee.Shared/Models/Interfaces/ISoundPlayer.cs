using Midi;

namespace Orphee.Models.Interfaces
{
    public interface ISoundPlayer
    {
        // Properties

        // Methods
        void PlayNote(Note note);
        void UpdatePlayingInstrument(Instrument newPlayingInstrument);
    }
}
