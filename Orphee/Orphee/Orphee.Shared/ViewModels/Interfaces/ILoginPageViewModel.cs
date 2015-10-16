using Microsoft.Practices.Prism.Commands;

namespace Orphee.ViewModels.Interfaces
{
    /// <summary>
    /// LoginPageViewModel interface
    /// </summary>
    public interface ILoginPageViewModel
    {
        /// <summary>Logs the user to the server </summary>
        DelegateCommand LoginCommand { get; }
        /// <summary>Redirects to the previous page </summary>
        DelegateCommand BackCommand { get; }
        /// <summary>User name of the user </summary>
        string UserName { get; set; }
        /// <summary>User's password </summary>
        string Password { get; set; }
    }
}
