using Microsoft.Practices.Prism.Commands;

namespace Orphee.ViewModels.Interfaces
{
    public interface IParametersPageViewModel
    {
        DelegateCommand DeleteAccoutCommand { get; }
        DelegateCommand LogoutCommand { get; }
        DelegateCommand ResetPasswordCommand { get; }
        DelegateCommand GoBackCommand { get; }
    }
}
