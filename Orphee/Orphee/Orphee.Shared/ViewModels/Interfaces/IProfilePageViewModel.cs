using Windows.UI.Input;
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.Commands;

namespace Orphee.ViewModels.Interfaces
{
    public interface IProfilePageViewModel
    {
        DelegateCommand LoginButton { get; }
        DelegateCommand RegisterButton { get; }
        string DisconnectedMessage { get; }
        Visibility ButtonsVisibility { get; }
        Visibility ListViewVisibility { get; }
        void PlaySelectedFile(string fileName);
    }
}
