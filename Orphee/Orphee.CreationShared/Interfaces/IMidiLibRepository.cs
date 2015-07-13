using Midi;

namespace Orphee.CreationShared.Interfaces
{
    public interface IMidiLibRepository
    {
        // Properties

        // Methods
        void PlayNote(Note note);
        void UpdatePlayingInstrument(Instrument newPlayingInstrument);
    }
}
