using Windows.UI.Xaml;
using Midi;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    /// <summary>
    /// This class contains the informations related to the graphical
    /// representation of a note
    /// </summary>
    public class ToggleButtonNote : IToggleButtonNote
    {
        /// <summary>Value representing the width of the toggleButtonNote </summary>
        public int Width { get; private set; }
        /// <summary>Value representing the height of the toggleButtonNote </summary>
        public int Height { get; private set; }
        /// <summary>Value representing the line index of the toggleButtonNote </summary>
        public int LineIndex { get; set; }
        /// <summary>Value representing the column index of the toggleButtonNote </summary>
        public int ColumnIndex { get; set; }
        /// <summary>Value representing the note related to the toggleButtonNote </summary>
        public Note Note { get; set; }
        /// <summary>Value bound to the toggleButtonNote's isCheck trigger</summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ToggleButtonNote()
        {
            this.Width = (int) (Window.Current.Bounds.Width / 18);
            this.Height = (int) (Window.Current.Bounds.Height / 18);
        }
    }
}
