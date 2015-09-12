using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;
using Orphee.RestApiManagement.Annotations;
using Orphee.RestApiManagement.Interfaces;

namespace Orphee.RestApiManagement
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
        public List<Message> PendingMessageList { get; set; } 
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

        public string PicturePath { get; set; }

        public User()
        {
            this.PendingMessageList = new List<Message>();
            this.PendingFriendList = new List<User>();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
