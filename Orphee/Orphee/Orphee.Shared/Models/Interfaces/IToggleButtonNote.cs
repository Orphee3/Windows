using Midi;

namespace Orphee.Models.Interfaces
{
    public interface IToggleButtonNote
    {
        // Methods

        // Properties
        int LineIndex { get; set; }
        int ColumnIndex { get; set; }
        Note Note { get; set; }
        bool IsChecked { get; set; }
    }
}
