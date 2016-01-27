using Midi;
using Newtonsoft.Json;

namespace Orphee.CreationShared.Interfaces
{
    public interface INoteToSend
    {
        [JsonProperty(PropertyName = "LineIndex")]
        int LineIndex { get; set; }
        [JsonProperty(PropertyName = "ColumnIndex")]
        int ColumnIndex { get; set; }
        [JsonProperty(PropertyName = "Note")]
        Note Note { get; set; }
        [JsonProperty(PropertyName = "Channel")]
        Channel Channel { get; set; }
        [JsonProperty(PropertyName = "Octave")]
        int Octave { get; set; }
    }
}
