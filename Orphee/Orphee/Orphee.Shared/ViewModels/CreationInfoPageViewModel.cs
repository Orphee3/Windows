﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using MidiDotNet.ImportModule.Interfaces;
using Orphee.CreationShared.Interfaces;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Models;
using Orphee.ViewModels.Interfaces;
using Orphee.RestApiManagement.Senders.Interfaces;

namespace Orphee.ViewModels
{
    /// <summary>
    /// CreationInfoPage view model
    /// </summary>
    public class CreationInfoPageViewModel : ViewModel, ICreationInfoPageViewModel
    {
        /// <summary>Redirects to the previous page </summary>
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand PlayCommand { get; private set; }
        /// <summary>List of comments related to the creation </summary>
        public ObservableCollection<Comment> CommentList { get; private set; }
        private string _creationName;
        private Creation _creation;
        private int _commentNumber;
        /// <summary>Number of comments related to the creation </summary>
        public int CommentNumber
        {
            get { return this._commentNumber; }
            set
            {
                if (this._commentNumber != value)
                    SetProperty(ref this._commentNumber, value);
            }
        }
        /// <summary>Name of the creation </summary>
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
        /// <summary>Number of like related to the creation</summary>
        public int LikeNumber
        {
            get { return this._likeNumber; }
            set
            {
                if (this._likeNumber != value)
                    SetProperty(ref this._likeNumber, value);
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
                if (!RestApiManagerBase.Instance.IsConnected)
                {
                    App.MyNavigationService.Navigate("Login", null);
                    return;
                }
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
            if (!RestApiManagerBase.Instance.NotificationRecieiver.IsInternet())
            {
                DisplayMessage("Connexion unavailable");
                return;
            }
            List<Comment> commentList;
            try
            {
                commentList = await this._getter.GetInfo<List<Comment>>(RestApiManagerBase.Instance.RestApiPath["comment"] + "/creation/" + this._creation.Id);
            }
            catch (Exception)
            {
                DisplayMessage("Request failed");
                return;
            }
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

        /// <summary>
        /// Updates the comment list
        /// </summary>
        /// <param name="pendingCommentList"></param>
        public async void UpdateCommentList(List<Comment> pendingCommentList)
        {
            if (pendingCommentList.Any(c => c.CreationId == this._creation.Id))
            {
                List<Comment> commentList;
                try
                {
                    commentList = await this._getter.GetInfo<List<Comment>>(RestApiManagerBase.Instance.RestApiPath["comment"] + "/creation/" + this._creation.Id);
                }
                catch (Exception)
                {
                    DisplayMessage("This comments was not sent");
                    return;
                }
                foreach (var comment in commentList)
                    if (!this.CommentList.Any(c => c.Id == comment.Id))
                    {
                        this.CommentList.Insert(0, comment);
                        this.CommentNumber++;
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
                orpheeFile = await this._importer.ImportFileFromNet(this._creation.GetUrl, this._creation.Name);
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

        private async void DisplayMessage(string message)
        {
            var messageDialog = new MessageDialog(message);

            await messageDialog.ShowAsync();
        }
    }
}
