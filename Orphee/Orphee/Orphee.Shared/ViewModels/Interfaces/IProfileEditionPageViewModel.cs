using Microsoft.Practices.Prism.Commands;

namespace Orphee.ViewModels.Interfaces
{
    public interface IProfileEditionPageViewModel
    {
        DelegateCommand GoBackCommand { get; }
        DelegateCommand ChangePictureCommand { get; }
    }
}
