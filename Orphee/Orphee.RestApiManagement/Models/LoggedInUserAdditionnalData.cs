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
        protected bool _hasReceivedRoomListNotification;
        /// <summary>True if a friend validation notification was received. False otherwise </summary>
        public bool HasReceivedNewComerNotification
        {
            get { return this._hasReceivedNewComerNotification; }
            set
            {
                if (this._hasReceivedNewComerNotification != value)
                {
                    this._hasReceivedNewComerNotification = value;
                    OnPropertyChanged(nameof(this._hasReceivedNewComerNotification));
                }
            }
        }
        protected bool _hasReceivedNewComerNotification;

        public bool HasReceivedBigBangNotification
        {
            get { return this._hasReceivedBigBangNotification; }
            set
            {
                if (this._hasReceivedBigBangNotification != value)
                {
                    this._hasReceivedBigBangNotification = value;
                    OnPropertyChanged(nameof(this._hasReceivedBigBangNotification));
                }
            }
        }
        protected bool _hasReceivedBigBangNotification;

        public bool HasReceivedKickNotification
        {
            get { return this._hasReceivedKickNotification; }
            set
            {
                if (this._hasReceivedKickNotification != value)
                {
                    this._hasReceivedKickNotification = value;
                    OnPropertyChanged(nameof(this._hasReceivedKickNotification));
                }
            }
        }
        protected bool _hasReceivedKickNotification;

        public bool HasReceivedLeavingNotification
        {
            get { return this._hasReceivedLeavingNotification; }
            set
            {
                if (this._hasReceivedLeavingNotification != value)
                {
                    this._hasReceivedLeavingNotification = value;
                    OnPropertyChanged(nameof(this._hasReceivedLeavingNotification));
                }
            }
        }
        protected bool _hasReceivedLeavingNotification;
        public bool HasReceivedRoomListNotification
        {
            get { return this._hasReceivedRoomListNotification; }
            set
            {
                if (this._hasReceivedRoomListNotification != value)
                {
                    this._hasReceivedRoomListNotification = value;
                    OnPropertyChanged(nameof(this._hasReceivedRoomListNotification));
                }
            }
        }

        protected bool _receivedOrpheeFileFromHostNotifacation;
        public bool ReceivedOrpheeFileFromHostNotifacation
        {
            get { return this._receivedOrpheeFileFromHostNotifacation; }
            set
            {
                if (this._receivedOrpheeFileFromHostNotifacation != value)
                {
                    this._receivedOrpheeFileFromHostNotifacation = value;
                    OnPropertyChanged(nameof(this._receivedOrpheeFileFromHostNotifacation));
                }
            }
        }

        protected bool _hasReceivedNewRoomNotification;
        public bool HasReceivedNewRoomNotification
        {
            get { return this._hasReceivedNewRoomNotification; }
            set
            {
                if (this._hasReceivedNewRoomNotification != value)
                {
                    this._hasReceivedNewRoomNotification = value;
                    OnPropertyChanged(nameof(this._hasReceivedNewRoomNotification));
                }
            }
        }
        protected bool _hasReceivedCreationInfoNotification;
        public bool HasReceivedCreationInfoNotification
        {
            get { return this._hasReceivedCreationInfoNotification; }
            set
            {
                if (this._hasReceivedCreationInfoNotification != value)
                {
                    this._hasReceivedCreationInfoNotification = value;
                    OnPropertyChanged(nameof(this._hasReceivedCreationInfoNotification));
                }
            }
        }
        public string InfoType { get; set; }
        public string ReceivedInfo { get; set; }
        public string ActualRoomId { get; set; }
        public object ActualSharedOrpheeFile { get; set; }
        public string NewComer { get; set; }
        public string LeavingUser { get; set; }

        public List<Room> RoomList { get; set; }
    }
}
