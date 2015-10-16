using Microsoft.Practices.Prism.Commands;

namespace Orphee.ViewModels.Interfaces
{
    /// <summary>
    /// RegisterPageViewModel interface
    /// </summary>
    public interface IRegisterPageViewModel
    {
        /// <summary>Register command </summary>
        DelegateCommand RegisterCommand { get; }
        /// <summary>Back command </summary>
        DelegateCommand BackCommand { get; }
        /// <summary>New account userName</summary>
        string UserName { get; set; }
        /// <summary>New account password </summary>
        string Password { get; set; }
        /// <summary>New account mail adress </summary>
        string MailAdress { get; set; }
    }
}
