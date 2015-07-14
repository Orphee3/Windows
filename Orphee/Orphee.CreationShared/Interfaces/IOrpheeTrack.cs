using System.Collections.Generic;
using System.Collections.ObjectModel;
using Midi;

namespace Orphee.CreationShared.Interfaces
{
    public interface IOrpheeTrack
    {
        // Methods

        // Properties
        IList<ObservableCollection<IToggleButtonNote>> NoteMap { get; }
        IList<IOrpheeNoteMessage> OrpheeNoteMessageList { get; set; }
        Instrument CurrentInstrument { get; set; }
        IPlayerParameters PlayerParameters { get; set; }
        Channel Channel { get; }
        int TrackPos { get; }
        uint TrackLength { get; }
    }
}
