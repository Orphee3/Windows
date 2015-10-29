using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;
using Orphee.RestApiManagement.Annotations;
using Orphee.RestApiManagement.Models.Interfaces;

namespace Orphee.RestApiManagement.Models
{
    /// <summary>
    /// Class containing all the conversation data
    /// </summary>
    public class Conversation : IConversation, INotifyPropertyChanged
    {
        /// <summary>Conversation id </summary>
        public string Id { get; set; }
        /// <summary>Name of the conversation </summary>
        public string Name { get; set; }

        private string _lastMessagePreview;

        /// <summary>Preview of the last message reveived </summary>
        public string LastMessagePreview
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
        /// <summary>Conversation participants </summary>
        public JArray Users { get; set; }

        public JArray TemporaryUsers { get; set; }

        /// <summary>Messages contained in the conversation </summary>
        public JArray MessageList { get; set; }

        private DateTime _lastMessageDate;
        /// <summary>Date of the last message </summary>
        public DateTime LastMessageDate
        {
            get { return this._lastMessageDate; }
            set
            {
                if (this._lastMessageDate != value)
                {
                    this._lastMessageDate = value;
                    OnPropertyChanged(nameof(this.LastMessageDate));
                    this.LastMessageDateString = this.LastMessageDate.ToString("HH:mm");
                }
            }
        }
        /// <summary>Conversation participants </summary>
        public List<User> UserList { get; set; }
        /// <summary>Messages contained in the conversation </summary>
        public List<Message> Messages { get; set; }
        /// <summary>Conversation picture source </summary>
        public string ConversationPictureSource { get; set; }

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


        /// <summary>
        /// Constructor
        /// </summary>
        public Conversation()
        {
            this.UserList = new List<User>();
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
