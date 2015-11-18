using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Orphee.CreationShared.Interfaces;
using Orphee.RestApiManagement.Annotations;

namespace Orphee.CreationShared
{
    public class OrpheeTrackUI : IOrpheeTrackUI, INotifyPropertyChanged
    {
        private SolidColorBrush _trackColor;
        public SolidColorBrush TrackColor
        {
            get { return this._trackColor; }
            set
            {
                if (this._trackColor != value)
                {
                    this._trackColor = value;
                    OnPropertyChanged(nameof(TrackColor));
                }
            }
        }

        private Visibility _trackVisibility;

        public Visibility TrackVisibility
        {
            get { return this._trackVisibility; }
            set
            {
                if (this._trackVisibility != value)
                {
                    this._trackVisibility = value;
                    OnPropertyChanged(nameof(TrackVisibility));
                }
            }
        }

        private readonly IColorManager _colorManager;
        public OrpheeTrackUI(IColorManager colorManager)
        {
            this._colorManager = colorManager;
        }
        public void InitProperties(int trackPos)
        {
            this.TrackColor = this._colorManager.ColorList[trackPos];
            this.TrackVisibility = trackPos == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
