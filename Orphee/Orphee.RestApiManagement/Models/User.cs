﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Newtonsoft.Json.Linq;
using Orphee.RestApiManagement.Annotations;
using Orphee.RestApiManagement.Models.Interfaces;

namespace Orphee.RestApiManagement.Models
{
    public class User : IUser, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public JArray Friends { get; set; }
        public string Picture { get; set; }
        public DateTime DateCreaion { get; set; }
        public bool IsChecked { get; set; }
        public JArray Comments { get; set; }
        public JArray Likes { get; set; }
        public JArray Creations { get; set; }
        public List<User> PendingFriendList { get; set; }
        public List<Comment> PendingCommentList { get; set; } 
        public List<User> FriendList { get; set; } 
        public List<Message> PendingMessageList { get; set; }
        private bool _hasReceivedCommentNotification;
        public bool HasReceivedCommentNotification
        {
            get { return _hasReceivedCommentNotification; }
            set
            {
                if (this._hasReceivedCommentNotification != value)
                {
                    this._hasReceivedCommentNotification = value;
                    OnPropertyChanged(nameof(_hasReceivedCommentNotification));
                }
            }
        }
        private bool _hasReceivedFriendNotification;
        public bool HasReceivedFriendNotification
        {
            get { return _hasReceivedFriendNotification; }
            set
            {
                if (this._hasReceivedFriendNotification != value)
                {
                    this._hasReceivedFriendNotification = value;
                    OnPropertyChanged(nameof(_hasReceivedFriendNotification));
                }
            }
        }
        private bool _hasReceivedMessageNotification;
        public bool HasReceivedMessageNotification
        {
            get { return _hasReceivedMessageNotification; }
            set
            {
                if (this._hasReceivedMessageNotification != value)
                {
                    this._hasReceivedMessageNotification = value;
                    OnPropertyChanged(nameof(_hasReceivedMessageNotification));
                }
            }
        }
        private bool _hasReceivedFriendValidationNotification;
        public bool HasReceivedFriendValidationNotification
        {
            get { return _hasReceivedFriendValidationNotification; }
            set
            {
                if (this._hasReceivedFriendValidationNotification != value)
                {
                    this._hasReceivedFriendValidationNotification = value;
                    OnPropertyChanged(nameof(_hasReceivedFriendValidationNotification));
                }
            }
        }
        private bool _hasReceivedFriendConfirmationNotification;
        public bool HasReceivedFriendConfirmationNotification
        {
            get { return this._hasReceivedFriendConfirmationNotification; }
            set
            {
                if (this._hasReceivedFriendConfirmationNotification != value)
                {
                    this._hasReceivedFriendConfirmationNotification = value;
                    OnPropertyChanged(nameof(this._hasReceivedFriendConfirmationNotification));
                }
            }
        }
        public Visibility AddButtonVisibility { get; set; }
        private bool _pictureHasBeenUplaodedWithSuccess;
        public bool PictureHasBeenUplaodedWithSuccess
        {
            get { return this._pictureHasBeenUplaodedWithSuccess; }
            set
            {
                if (this._pictureHasBeenUplaodedWithSuccess != value)
                {
                    this._pictureHasBeenUplaodedWithSuccess = value;
                    OnPropertyChanged(nameof(this._pictureHasBeenUplaodedWithSuccess));
                }
            }
        }
        public User()
        {
            this.PendingMessageList = new List<Message>();
            this.PendingFriendList = new List<User>();
            this.PendingCommentList = new List<Comment>();
;            if (string.IsNullOrEmpty(this.Picture))
                this.Picture = "/Assets/defaultUser.png";
            this.AddButtonVisibility = Visibility.Visible;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
