using System.ComponentModel;
using Windows.UI.Xaml;

namespace Orphee.CreationShared.Interfaces
{
    public interface IMyRectangle : INotifyPropertyChanged
    {
        Visibility IsSelectionRectangleVisible { get; set; }
        double IsRectangleVisible { get; set; }
    }
}
