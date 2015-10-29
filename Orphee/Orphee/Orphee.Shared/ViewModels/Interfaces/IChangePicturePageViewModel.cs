using Microsoft.Practices.Prism.Commands;

namespace Orphee.ViewModels.Interfaces
{
    /// <summary>
    /// ProfileEditionPageViewModel interface
    /// </summary>
    public interface IChangePicturePageViewModel
    {
        /// <summary>Redirects to the previous page </summary>
        DelegateCommand GoBackCommand { get; }
        /// <summary>Enables the posibility to change the user's picture </summary>
        DelegateCommand ChangePictureCommand { get; }
    }
}
