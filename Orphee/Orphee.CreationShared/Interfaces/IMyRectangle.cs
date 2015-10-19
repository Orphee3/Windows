using System.Collections.Specialized;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Orphee.CreationShared.Interfaces
{
    public interface IMyRectangle : INotifyPropertyChanged
    {
        SolidColorBrush RectangleBackgroundColor { get; set; }
        Visibility IsSelectionRectangleVisible { get; set; }
    }
}
