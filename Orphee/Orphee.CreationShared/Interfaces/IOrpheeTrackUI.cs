using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Orphee.CreationShared.Interfaces
{
    public interface IOrpheeTrackUI
    {
        SolidColorBrush TrackColor { get; set; }
        Visibility TrackVisibility { get; set; }

        void InitProperties(int trackPos);
    }
}
