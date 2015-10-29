using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Orphee.RestApiManagement.Models;

namespace Orphee.ViewModels.Interfaces
{
    /// <summary>
    /// CreationInfoPageViewModel interface
    /// </summary>
    public interface ICreationInfoPageViewModel
    {
        /// <summary>Redirects to the previous page </summary>
        DelegateCommand GoBackCommand { get; }
        DelegateCommand PlayCommand { get; }
        Creation Creation { get; }
        /// <summary>List of comments related to the creation </summary>
        ObservableCollection<Comment> CommentList { get; }
        /// <summary>User picture source </summary>
        string UserPictureSource { get; }

        /// <summary>
        /// Sends a comment to the remote server
        /// </summary>
        /// <param name="text"></param>
        void SendComment(string text);

        /// <summary>
        /// Updates the comment list
        /// </summary>
        /// <param name="commentList"></param>
        void UpdateCommentList(List<Comment> commentList);
    }
}
