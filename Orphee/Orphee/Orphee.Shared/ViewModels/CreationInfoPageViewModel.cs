using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Interfaces;
using Orphee.RestApiManagement.Models;
using Orphee.RestApiManagement.Senders.Interfaces;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class CreationInfoPageViewModel : ViewModel, ICreationInfoPageViewModel
    {
        public DelegateCommand GoBackCommand { get; private set; }
        public ObservableCollection<Comment> CommentList { get; private set; }
        private string _creationName;
        private Creation _creation;
        private int _commentNumber;

        public int CommentNumber
        {
            get { return this._commentNumber; }
            set
            {
                if (this._commentNumber != value)
                    SetProperty(ref this._commentNumber, value);
            }
        }
        public string CreationName
        {
            get { return this._creationName; }
            set
            {
                if (this._creationName != value)
                    SetProperty(ref this._creationName, value);
            }
        }

        private int _likeNumber;

        public int LikeNumber
        {
            get { return this._likeNumber; }
            set
            {
                if (this._likeNumber != value)
                    SetProperty(ref this._likeNumber, value);
            }
        }
        private readonly ICommentSender _commentSender;
        private readonly ICreationCommentGetter _creationCommentGetter;

        public CreationInfoPageViewModel(ICommentSender commentsender, ICreationCommentGetter creationCommentGetter)
        {
            this._creationCommentGetter = creationCommentGetter;
            this._commentSender = commentsender;
            this.GoBackCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
            this.CommentList = new ObservableCollection<Comment>();
        }

        public async void SendComment(string newComment)
        {
            if (newComment.Any())
            {
                if (!RestApiManagerBase.Instance.IsConnected)
                {
                    App.MyNavigationService.Navigate("Login", null);
                    return;
                }
                this.CommentNumber++;
                this.CommentList.Insert(0, new Comment {Message = newComment});
                var result = await this._commentSender.SendComment(newComment, this._creation.Id);
            }
        }

        public async override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            this.CommentList.Clear();
            this.CommentNumber = 0;
            this.LikeNumber = 0;
            this._creation = navigationParameter as Creation;
            this.CreationName = this._creation.Name.Split('.')[0];
            var commentList = await this._creationCommentGetter.GetCreationComments(this._creation.Id);
            if (commentList == null)
                return;
            foreach (var comment in commentList)
            {
                if (this.CommentList.Count(c => c.Id == comment.Id) == 0)
                {
                    this.CommentNumber++;
                    this.CommentList.Insert(0, comment);
                }
            }
        }
    }
}
