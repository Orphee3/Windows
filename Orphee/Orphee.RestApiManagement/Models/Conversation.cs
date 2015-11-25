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
    /// Class containing all the conversation data
    /// </summary>
    [DataContract]
    public class Conversation : IConversation, INotifyPropertyChanged
    {
        /// <summary>Conversation id </summary>
        [DataMember]
        public string Id { get; set; }
        /// <summary>Name of the conversation </summary>
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        private Message _lastMessagePreview;

        /// <summary>Preview of the last message reveived </summary>
        public Message LastMessagePreview
        {
            get { return this._lastMessagePreview; }
            set
            {
                if (this._lastMessagePreview != value)
                {
                    this._lastMessagePreview = value;
                    OnPropertyChanged(nameof(this.LastMessagePreview));
                }
            }
        }

        public JArray TemporaryUsers { get; set; }

        /// <summary>Messages contained in the conversation </summary>
        public JArray MessageList { get; set; }
        public bool IsPrivate { get; set; }
        /// <summary>Conversation participants </summary>
        [DataMember]
        public List<UserBase> UserList { get; set; }
        /// <summary>Messages contained in the conversation </summary>
        [DataMember]
        public List<Message> Messages { get; set; }
        /// <summary>Conversation picture source </summary>
        [DataMember]
        public string ConversationPictureSource { get; set; }
        [DataMember]
        private string _lastMessageDateString;
        public string LastMessageDateString
        {
            get { return this._lastMessageDateString; }
            set
            {
                if (this._lastMessageDateString != value)
                {
                    this._lastMessageDateString = value;
                    OnPropertyChanged(nameof(this.LastMessageDateString));
                }
            }
        }
        [DataMember]
        public bool IsNew { get; set; }

        private bool _hasReceivedNewMessage;
        [DataMember]
        public bool HasReceivedNewMessage
        {
            get { return this._hasReceivedNewMessage; }
            set
            {
                if (this._hasReceivedNewMessage != value)
                {
                    this._hasReceivedNewMessage = value;
                    this.NotificationDotVisibility = this._hasReceivedNewMessage ? Visibility.Visible : Visibility.Collapsed;
                    OnPropertyChanged(nameof(HasReceivedNewMessage));
                }
            }
            
        }

        private Visibility _notificationDotVisibility = Visibility.Collapsed;
        [DataMember]
        public Visibility NotificationDotVisibility
        {
            get { return this._notificationDotVisibility; }
            set
            {
                if (this._notificationDotVisibility != value)
                {
                    this._notificationDotVisibility = value;
                    OnPropertyChanged(nameof(NotificationDotVisibility));
                }
            }

        }
        /// <summary>
        /// Constructor
        /// </summary>
        public Conversation()
        {
            this.UserList = new List<UserBase>();
            this.Messages = new List<Message>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
