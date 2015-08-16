using Windows.UI.Xaml.Controls;
using Microsoft.Practices.Prism.Commands;

namespace Orphee.ViewModels.UI.Interfaces
{
    public interface IMyBottomAppBarViewModel
    {
        DelegateCommand<Button> ButtonCommand { get; }
    }
}
