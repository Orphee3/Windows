using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Orphee.RestApiManagement.Models
{
    /// <summary>
    /// Class respecting the server's user representation
    /// </summary>
    [DataContract]
    public class LoggedUser : LoggedInUserAdditionnalData
    {
        [DataMember]
        private bool? _wasNewPictureSent;

        /// <summary>
        /// Constructor
        /// </summary>
        public LoggedUser()
        {
            this._wasNewPictureSent = null;
            this.PendingMessageList = new List<Message>();
            this.PendingFriendList = new List<UserBase>();
            this.PendingCommentList = new List<Comment>();
            this.FriendList = new List<UserBase>();
            this.ConversationList = new List<Conversation>();
            this.NotificationList = new List<News>();
        }
    }
}
