using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using MidiDotNet.ImportModule.Interfaces;
using Newtonsoft.Json;
using Orphee.CreationShared.Interfaces;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels.Interfaces;
using Orphee.RestApiManagement.Senders.Interfaces;
using Q42.WinRT.Data;

namespace Orphee.ViewModels
{
    /// <summary>
    /// CreationInfoPage view model
    /// </summary>
    public class CreationInfoPageViewModel : ViewModelExtend, ICreationInfoPageViewModel
    {
        /// <summary>Redirects to the previous page </summary>
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand PlayCommand { get; private set; }
        /// <summary>List of comments related to the creation </summary>
        public ObservableCollection<Comment> CommentList { get; private set; }

        public Creation Creation { get; private set; }
        private bool? _isLiked;
        public bool? IsLiked
        {
            get
            {
                return this._isLiked;
            }
            set
            {
                if (this._isLiked != value)
                {
                    SetProperty(ref this._isLiked, value);
                }
            }
        }
        /// <summary>User picture source </summary>
        public string UserPictureSource { get; private set; }
        private readonly IGetter _getter;
        private readonly ICommentSender _commentSender;
        private readonly IOrpheeFileImporter _importer;
        private readonly ISoundPlayer _player;

        /// <summary>
        /// Constructor initializing getter and commentSender
        /// through dependency injection
        /// </summary>
        /// <param name="getter">Manages the sending of the "Get" requests</param>
        /// <param name="commentSender">Manages the comment sending to the remote server</param>
        public CreationInfoPageViewModel(IGetter getter, ICommentSender commentSender, IOrpheeFileImporter fileImporter, ISoundPlayer player)
        {
            this._getter = getter;
            this._importer = fileImporter;
            this._commentSender = commentSender;
            this._player = player;
            SetProgressRingVisibility(true);
            this.UserPictureSource = RestApiManagerBase.Instance.IsConnected ? RestApiManagerBase.Instance.UserData.User.Picture : "/Assets/defaultUser.png";
            this.PlayCommand = new DelegateCommand(PlayCommandExec);
            this.GoBackCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
            this.CommentList = new ObservableCollection<Comment>();
        }

        /// <summary>
        /// Sends a comment to the remote server
        /// </summary>
        /// <param name="newComment"></param>
        public async void SendComment(string newComment)
        {
            if (newComment.Any())
            {
                var result = false;
                if (!RestApiManagerBase.Instance.IsConnected)
                {
                    App.MyNavigationService.Navigate("Login", null);
                    return;
                }
                try
                {
                    result = await this._commentSender.SendComment(newComment, this.Creation.Id);
                }
                catch (Exception e)
                {
                   DisplayMessage("An error has occured. The comment wasn't sent");
                }
                if (!result)
                    DisplayMessage("Comment was not sent");
            }
        }

        public async override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            this.CommentList.Clear();
            this.Creation = JsonConvert.DeserializeObject<Creation>(navigationParameter as string);
            this.Creation.Name = this.Creation.Name.Split('.')[0];
            if (RestApiManagerBase.Instance.IsConnected)
                this.IsLiked = RestApiManagerBase.Instance.UserData.User.Likes.Any(l => l.ToString() == this.Creation.Id);
            if (!RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
                DisplayMessage("Connexion unavailable");
            List<Comment> commentList;
            try
            {
                commentList = await DataCache.GetAsync("CreationPage-" + this.Creation.Id, async () => await this._getter.GetInfo<List<Comment>>(RestApiManagerBase.Instance.RestApiPath["comment"] + "/creation/" + this.Creation.Id), DateTime.Now.AddHours(1));
            }
            catch (Exception)
            {
                DisplayMessage("Request failed");
                SetProgressRingVisibility(false);
                return;
            }
            if (commentList == null)
                return;
            foreach (var comment in commentList.Where(comment => this.CommentList.Count(c => c.Id == comment.Id) == 0))
            {
                this.Creation.NumberOfComment++;
                this.CommentList.Insert(0, comment);
            }
            SetProgressRingVisibility(false);
        }

        /// <summary>
        /// Updates the comment list
        /// </summary>
        /// <param name="pendingCommentList"></param>
        public void UpdateCommentList(List<Comment> pendingCommentList)
        {
            if (pendingCommentList.Any(c => c.CreationId == this.Creation.Id))
            {
                foreach (var comment in pendingCommentList)
                    if (this.CommentList.All(c => c.Id != comment.Id))
                    {
                        this.CommentList.Insert(0, comment);
                        this.Creation.NumberOfComment++;
                    }
            }
        }

        private async void PlayCommandExec()
        {
            if (!RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
            {
                DisplayMessage("Connexion unavailable");
                return;
            }
            IOrpheeFile orpheeFile;
            try
            {
                orpheeFile = await this._importer.ImportFileFromNet(this.Creation.GetUrl, this.Creation.Name);
            }
            catch (Exception)
            {
                DisplayMessage("Unable to play the piece");
                return;
            }

            if (orpheeFile != null)
            {
                this._player.SetPlayerParameters(orpheeFile.OrpheeTrackList[0].PlayerParameters);
                foreach (var track in orpheeFile.OrpheeTrackList)
                    this._player.PlayTrack(track.OrpheeNoteMessageList, track.CurrentInstrument, track.Channel);
            }
            else
                DisplayMessage("Unable to play the piece");
        }

        public async Task<bool?> LikeCommandExec()
        {
            if (!RestApiManagerBase.Instance.IsConnected)
            {
                App.MyNavigationService.Navigate("Login", null);
                return null;
            }
            User creator = null;
            var request = this._isLiked == false ? RestApiManagerBase.Instance.RestApiPath["like"] + this.Creation.Id : RestApiManagerBase.Instance.RestApiPath["dislike"] + this.Creation.Id;
            try
            {
                creator = await this._getter.GetInfo<User>(request);
            }
            catch (Exception)
            {
                DisplayMessage("Like wasn't sent");
            }
            if (creator != null)
            {
                this._isLiked = !this._isLiked;
                this.Creation.NumberOfLike += this._isLiked == true ? 1 : -1;
                return this._isLiked;
            }
            return null;
        }
    }
}
