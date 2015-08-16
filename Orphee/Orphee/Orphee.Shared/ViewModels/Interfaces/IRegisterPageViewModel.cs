using Microsoft.Practices.Prism.Commands;

namespace Orphee.ViewModels.Interfaces
{
    public interface IRegisterPageViewModel
    {
        DelegateCommand RegisterCommand { get; }
        DelegateCommand BackCommand { get; }
        string UserName { get; set; }
        string Password { get; set; }
        string MailAdress { get; set; }
    }
}
