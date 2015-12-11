using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Windows.UI.Xaml.Media;
using Orphee.Models;
using Orphee.RestApiManagement.Annotations;

namespace Orphee.RestApiManagement.Models
{
    [DataContract]
    public class LoggedInUserAdditionnalData : UserBase
    {
        //[DataMember]
        public List<UserBase> FriendList { get; set; }
        //[DataMember]
        public List<News> NotificationList { get; set; }
        //[DataMember]
        public ObservableCollection<Conversation> ConversationList { get; set; }
        //[DataMember]
        public List<UserBase> PendingFriendList { get; set; }
        /// <summary>List of unviewes comments </summary>
        //[DataMember]
        public List<Comment> PendingCommentList { get; set; }
        //[DataMember]
        public List<Creation> CreationList { get; set; }
            /// <summary>List of unviewed messages </summary>
        //[DataMember]
        public List<Message> PendingMessageList { get; set; }
        //[DataMember]
        protected bool _hasReceivedCommentNotification;
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
        //[DataMember]
        protected bool _hasReceivedFriendNotification;
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
        //[DataMember]
        protected bool _hasReceivedMessageNotification;
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
        //[DataMember]
        protected bool _hasReceivedFriendConfirmationNotification;
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
    }
}
