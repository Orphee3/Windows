using Midi;

namespace Orphee.CreationShared.Interfaces
{
    public interface ISoundPlayer
    {
        // Properties

        // Methods
        void PlayNote(Note note);
        void UpdatePlayingInstrument(Instrument newPlayingInstrument);
    }
}
