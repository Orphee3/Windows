using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Midi;

namespace Orphee.CreationShared.Interfaces
{
    public interface IOrpheeTrack
    {
        // Properties
        ObservableCollection<ObservableCollection<IToggleButtonNote>> NoteMap { get; }
        IList<IOrpheeNoteMessage> OrpheeNoteMessageList { get; set; }
        Instrument CurrentInstrument { get; set; }
        Channel Channel { get; set; }
        Visibility TrackVisibility { get; set; }
        int TrackPos { get; set; }
        uint TrackLength { get; set; }
        string TrackName { get; set; }
        bool IsChecked { get; set; }

        // Methods
        void UpdateOrpheeTrack(IOrpheeTrack orpheeTrack);
        void UpdateCurrentInstrument(Instrument instrument);
        void ConvertNoteMapToOrpheeMessage();
    }
}
