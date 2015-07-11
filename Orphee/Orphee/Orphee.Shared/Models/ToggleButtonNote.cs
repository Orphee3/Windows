using Midi;
using Orphee.Models.Interfaces;

namespace Orphee.Models
{
    public class ToggleButtonNote : IToggleButtonNote
    {
        public int LineIndex { get; set; }
        public int ColumnIndex { get; set; }
        public Note Note { get; set; }
        public bool IsChecked { get; set; }
    }
}
