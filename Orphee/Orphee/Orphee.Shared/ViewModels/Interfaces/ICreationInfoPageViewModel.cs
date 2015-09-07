using System.Collections.Generic;
using Microsoft.Practices.Prism.Commands;

namespace Orphee.ViewModels.Interfaces
{
    public interface ICreationInfoPageViewModel
    {
        DelegateCommand GoBackCommand { get; }
        List<string> CommentList { get; }
    }
}
