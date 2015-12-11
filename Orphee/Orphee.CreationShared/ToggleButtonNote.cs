using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Midi;
using Orphee.CreationShared.Interfaces;
using Orphee.RestApiManagement.Annotations;

namespace Orphee.CreationShared
{
    /// <summary>
    /// This class contains the informations related to the graphical
    /// representation of a note
    /// </summary>
    public class ToggleButtonNote : IToggleButtonNote, INotifyPropertyChanged
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
        private double _isChecked;
        /// <summary>Value bound to the toggleButtonNote's isCheck trigger</summary>
        public double IsChecked
        {
            get { return this._isChecked; }
            set
            {
                if (this._isChecked != value)
                {
                    this._isChecked = value;
                    OnPropertyChanged(nameof(IsChecked));
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ToggleButtonNote()
        {
            this.Width = (int)(Window.Current.Bounds.Width / 11);
            this.Height = (int)(Window.Current.Bounds.Height / 18.5);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
