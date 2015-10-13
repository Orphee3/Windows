using Windows.UI.Xaml;
using Midi;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    public class ToggleButtonNote : IToggleButtonNote
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int LineIndex { get; set; }
        public int ColumnIndex { get; set; }
        public Note Note { get; set; }
        public bool IsChecked { get; set; }

        public ToggleButtonNote()
        {
            this.Width = (int) (Window.Current.Bounds.Width / 18);
            this.Height = (int) (Window.Current.Bounds.Height / 18);
        }
    }
}
