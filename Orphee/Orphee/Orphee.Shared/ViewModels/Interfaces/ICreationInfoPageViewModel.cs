using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Orphee.RestApiManagement.Models;

namespace Orphee.ViewModels.Interfaces
{
    public interface ICreationInfoPageViewModel
    {
        DelegateCommand GoBackCommand { get; }
        ObservableCollection<Comment> CommentList { get; }
        void SendComment(string text);
        string CreationName { get; }
        int CommentNumber { get; set; }
        int LikeNumber { get; set; }
        string UserPictureSource { get; }
        void UpdateCommentList(List<Comment> commentList);
    }
}
