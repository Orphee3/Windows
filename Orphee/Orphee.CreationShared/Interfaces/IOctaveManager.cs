using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Orphee.CreationShared.Interfaces
{
    public interface IOctaveManager
    {
        [JsonProperty(PropertyName = "OctaveMap")]
        ObservableCollection<ObservableCollection<IToggleButtonNote>> OctaveMap { get; set; }
        int OctavePos { get; }
        string OctavePosString { get; }
        IOctaveManagerUI OctaveManagerUI { get; }
    }
}
