using Midi;

namespace Orphee.CreationShared.Interfaces
{
    /// <summary>
    /// ToogleButtonNote interface
    /// </summary>
    public interface IToggleButtonNote
    {
        // Methods

        // Properties

        /// <summary>Value representing the width of the toggleButtonNote </summary>
        int Width { get; }
        /// <summary>Value representing the height of the toggleButtonNote </summary>
        int Height { get; }
        /// <summary>Value representing the line index of the toggleButtonNote </summary>
        int LineIndex { get; set; }
        /// <summary>Value representing the column index of the toggleButtonNote </summary>
        int ColumnIndex { get; set; }
        /// <summary>Value representing the note related to the toggleButtonNote </summary>
        Note Note { get; set; }
        /// <summary>Value bound to the toggleButtonNote's isCheck trigger</summary>
        bool IsChecked { get; set; }
    }
}
