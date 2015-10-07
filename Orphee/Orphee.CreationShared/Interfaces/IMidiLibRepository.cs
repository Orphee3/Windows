using System.Collections.Generic;
using Midi;

namespace Orphee.CreationShared.Interfaces
{
    public interface IMidiLibRepository
    {
        // Properties
        IPlayerParameters PlayerParameters { get; set; }

        // Methods
        void PlayNote(Note note, Channel channel);
        void UpdatePlayingInstrument(Channel channel, Instrument newPlayingInstrument);
        void UpdateTempo(uint tempo);
        void SetPlayerParameters(IPlayerParameters playerParameters);
        void PlayTrack(IList<IOrpheeNoteMessage> noteMessageList, Instrument instrument, Channel channel);
    }
}
