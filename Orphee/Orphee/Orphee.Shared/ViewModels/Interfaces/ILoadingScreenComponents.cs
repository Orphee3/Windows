using Windows.UI.Xaml;

namespace Orphee.ViewModels.Interfaces
{
    public interface ILoadingScreenComponents
    {
        bool IsProgressRingActive { get; set; }
        Visibility ProgressRingVisibility { get; set; }
    }
}
