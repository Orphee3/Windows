using Microsoft.Practices.Prism.Commands;

namespace Orphee.ViewModels.Interfaces
{
    public interface ILoginPageViewModel
    {
        DelegateCommand LoginCommand { get; }
        DelegateCommand BackCommand { get; }
        string UserName { get; set; }
        string Password { get; set; }
    }
}
