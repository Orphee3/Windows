using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media;

namespace Orphee.CreationShared.Interfaces
{
    public interface IColorManager
    {
        ObservableCollection<SolidColorBrush> ColorList { get; }
        int GetColorIndex(SolidColorBrush color);
    }
}
