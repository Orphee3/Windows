using System.Collections.Generic;
using System.Collections.ObjectModel;
using Midi;

namespace Orphee.CreationShared.Interfaces
{
    public interface IOrpheeTrack
    {
        // Properties
        IList<ObservableCollection<IToggleButtonNote>> NoteMap { get; }
        IList<IOrpheeNoteMessage> OrpheeNoteMessageList { get; set; }
        Instrument CurrentInstrument { get; set; }
        IPlayerParameters PlayerParameters { get; set; }
        Channel Channel { get; set; }
        int TrackPos { get; set; }
        uint TrackLength { get; set; }

        // Methods
        void UpdateOrpheeTrack(IOrpheeTrack orpheeTrack);
    }
}
