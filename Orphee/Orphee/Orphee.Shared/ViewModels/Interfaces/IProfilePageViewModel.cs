using Windows.UI.Xaml;
using Microsoft.Practices.Prism.Commands;

namespace Orphee.ViewModels.Interfaces
{
    public interface IProfilePageViewModel
    {
        string UserName { get; }
        int NumberOfCreations { get; }
        int NumberOfComments { get; }
        int NumberOfFollows { get; }
        int NumberOfFollowers { get; }
        Visibility DisconnectedStackPanelVisibility { get; }
        Visibility ConnectedStackPanelVisibility { get; }
        DelegateCommand LoginCommand { get; }
    }
}
