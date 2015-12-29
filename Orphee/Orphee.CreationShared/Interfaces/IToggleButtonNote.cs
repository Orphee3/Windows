using Midi;

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
        int Width { get; }
        /// <summary>Value representing the height of the toggleButtonNote </summary>
        int Height { get; }
        /// <summary>Value bound to the toggleButtonNote's isCheck trigger</summary>
        double IsChecked { get; set; }
    }
}
