using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Windows.UI.Xaml;
using Newtonsoft.Json.Linq;
using Orphee.RestApiManagement.Annotations;
using Orphee.RestApiManagement.Models.Interfaces;

namespace Orphee.RestApiManagement.Models
{
    /// <summary>
    /// Class respecting the server's user representation
    /// </summary>
    [DataContract]
    public class User : IUser, INotifyPropertyChanged
    {
        /// <summary>PropertyChange event </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>User id</summary>
        [DataMember]
        public string Id { get; set; }
        /// <summary>User UserName </summary>
        [DataMember]
        public string UserName { get; set; }
        /// <summary>User name </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>User friend list </summary>
        public List<string> Friends { get; set; }
        /// <summary>User picture path </summary>
        [DataMember]
        public string Picture { get; set; }
        /// <summary>User creation date  </summary>
        public DateTime DateCreaion { get; set; }
        /// <summary>True if the user ahs been selected for a new conversation. False otherwise </summary>
        public bool IsChecked { get; set; }
        /// <summary>User comment list </summary>
        public JArray Comments { get; set; }
        /// <summary>User like list </summary>
        [DataMember]
        public List<string> Likes { get; set; }
        /// <summary>User creation list </summary>
        [DataMember]
        public List<string> Creations { get; set; }
        /// <summary>List of pending friend asking </summary>
        [DataMember]
        public List<User> PendingFriendList { get; set; }
        /// <summary>List of unviewes comments </summary>
        [DataMember]
        public List<Comment> PendingCommentList { get; set; }
        /// <summary>List of unviewed messages </summary>
        [DataMember]
        public List<Message> PendingMessageList { get; set; }
        [DataMember]
        private bool _hasReceivedCommentNotification;
        /// <summary>True if a comment notification was received. False otherwise </summary>
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
        [DataMember]
        private bool _hasReceivedFriendNotification;
        /// <summary>True if a frien notification was received. False otherwise </summary>
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
        [DataMember]
        private bool _hasReceivedMessageNotification;
        /// <summary>True if a message notification was received. False otherwise </summary>
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
        [DataMember]
        private bool _hasReceivedFriendConfirmationNotification;
        /// <summary>True if a friend validation notification was received. False otherwise </summary>
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
        /// <summary> If this user is a friend if the logged user, the add button is hidden. It's visible otherwise </summary>
        [DataMember]
        public Visibility AddButtonVisibility { get; set; }
        private bool _pictureHasBeenUplaodedWithSuccess;
        /// <summary>True if a picture was uploaded with success. False otherwise </summary>
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
        [DataMember]
        public List<User> FriendList { get; set; }
        [DataMember]
        public List<News> NotificationList { get; set; }
        [DataMember]
        public List<Conversation> ConversationList { get; set; } 

        /// <summary>
        /// Constructor
        /// </summary>
        public User()
        {
            this.PendingMessageList = new List<Message>();
            this.PendingFriendList = new List<User>();
            this.PendingCommentList = new List<Comment>();
            this.FriendList = new List<User>();
            this.ConversationList = new List<Conversation>();
            this.NotificationList = new List<News>();
            if (string.IsNullOrEmpty(this.Picture))
                this.Picture = "ms-appx://" + "/Assets/defaultUser.png";
            this.AddButtonVisibility = Visibility.Visible;
        }

        /// <summary>
        /// Handling OnPropertyChanged event
        /// </summary>
        /// <param name="propertyName">Name of the property that has just changed</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
