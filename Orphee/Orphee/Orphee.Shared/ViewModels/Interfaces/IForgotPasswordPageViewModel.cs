using Microsoft.Practices.Prism.Commands;

namespace Orphee.ViewModels.Interfaces
{
    public interface IForgotPasswordPageViewModel
    {
        DelegateCommand SendCommand { get; }
        string UserMailAdress { get; set; }
    }
}
