using Microsoft.Practices.Prism.Commands;

namespace Orphee.ViewModels.Interfaces
{
    public interface ICreationInfoPageViewModel
    {
        DelegateCommand GoBackCommand { get; }
    }
}
