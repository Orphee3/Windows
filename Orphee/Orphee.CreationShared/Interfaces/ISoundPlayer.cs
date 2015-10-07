using System.Collections.Generic;
using Midi;

namespace Orphee.CreationShared.Interfaces
{
    public interface ISoundPlayer
    {
        // Properties

        // Methods
        void PlayNote(Note note, Channel channel);
        void UpdateCurrentInstrument(Instrument instrument, Channel channel);
        void UpdateTempo(uint tempo);
        IPlayerParameters GetPlayerParameters();
        void SetPlayerParameters(IPlayerParameters playerParameters);
        void PlayTrack(IList<IOrpheeNoteMessage> noteMessageList, Instrument instrument, Channel channel);
    }
}
