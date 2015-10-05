using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Models;
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
        public string UserPictureSource { get; private set; }
        private readonly IGetter _getter;

        public CreationInfoPageViewModel(IGetter getter)
        {
            this._getter = getter;
            this.UserPictureSource = RestApiManagerBase.Instance.IsConnected ? RestApiManagerBase.Instance.UserData.User.Picture : "/Assets/defaultUser.png";
            this.GoBackCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
            this.CommentList = new ObservableCollection<Comment>();
        }

        public async void SendComment(string newComment)
        {
            //if (newComment.Any())
            //{
            //    if (!RestApiManagerBase.Instance.IsConnected)
            //    {
            //        App.MyNavigationService.Navigate("Login", null);
            //        return;
            //    }
            //    var result = await this._commentSender.SendComment(newComment, this._creation.Id);
            //}
        }

        public async override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            this.CommentList.Clear();
            this.CommentNumber = 0;
            this.LikeNumber = 0;
            this._creation = navigationParameter as Creation;
            this.CreationName = this._creation.Name.Split('.')[0];
            var commentList = await this._getter.GetInfo<List<Comment>>(RestApiManagerBase.Instance.RestApiPath["comment"] + "/creation/" + this._creation.Id);
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

        public async void UpdateCommentList(List<Comment> pendingCommentList)
        {
            if (pendingCommentList.Any(c => c.CreationId == this._creation.Id))
            {
                var commentList = await this._getter.GetInfo<List<Comment>>(RestApiManagerBase.Instance.RestApiPath["comment"] + "/creation/" + this._creation.Id);
                foreach (var comment in commentList)
                    if (!this.CommentList.Any(c => c.Id == comment.Id))
                    {
                        this.CommentList.Insert(0, comment);
                        this.CommentNumber++;
                    }
            }
        }
    }
}
