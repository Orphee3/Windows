using Midi;
using Newtonsoft.Json;

namespace Orphee.CreationShared.Interfaces
{
    /// <summary>
    /// ToogleButtonNote interface
    /// </summary>
    public interface IToggleButtonNote : INoteToSend
    {
        // Methods

        // Properties

        /// <summary>Value representing the width of the toggleButtonNote </summary>
        [JsonProperty(PropertyName = "Width")]
        int Width { get; }
        /// <summary>Value representing the height of the toggleButtonNote </summary>
        [JsonProperty(PropertyName = "Height")]
        int Height { get; }
        /// <summary>Value bound to the toggleButtonNote's isCheck trigger</summary>
        [JsonProperty(PropertyName = "IsChecked")]
        double IsChecked { get; set; }
    }
}
