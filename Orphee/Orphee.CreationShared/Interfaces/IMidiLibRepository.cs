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
    }
}
