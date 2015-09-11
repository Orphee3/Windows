using Microsoft.Practices.Prism.Commands;

namespace Orphee.ViewModels.Interfaces
{
    public interface INewConversationConfigPageViewModel
    {
        DelegateCommand GoBackCommand { get; }
        DelegateCommand CreateConversationCommand { get; }
    }
}
