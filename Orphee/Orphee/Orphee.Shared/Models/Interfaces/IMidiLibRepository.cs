using Midi;

namespace Orphee.Models.Interfaces
{
    public interface IMidiLibRepository
    {
        // Properties

        // Methods
        void PlayNote(Note note);
        void UpdatePlayingInstrument(Instrument newPlayingInstrument);
    }
}
