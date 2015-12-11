using System.Collections.ObjectModel;

namespace Orphee.CreationShared.Interfaces
{
    public interface IOctaveManager
    {
        ObservableCollection<ObservableCollection<IToggleButtonNote>> OctaveMap { get; set; }
        int OctavePos { get; }
        string OctavePosString { get; }
        IOctaveManagerUI OctaveManagerUI { get; }
    }
}
