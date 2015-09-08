using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Orphee.RestApiManagement.Annotations;

namespace Orphee.ViewModels.Interfaces
{
    public interface ICreationInfoPageViewModel
    {
        DelegateCommand GoBackCommand { get; }
        ObservableCollection<string> CommentList { get; }
        void SendComment(string text);
    }
}
