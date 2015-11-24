using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using Orphee.RestApiManagement.Annotations;
using Orphee.RestApiManagement.Models.Interfaces;

namespace Orphee.RestApiManagement.Models
{
    /// <summary>
    /// Class containing all the creation
    /// related data
    /// </summary>
    [DataContract]
    public class Creation : ICreation, INotifyPropertyChanged
    {
        /// <summary>Creation id </summary>
        [DataMember]
        public string Id { get; set; }

        private string _name;

        /// <summary>Creation name</summary>
        [DataMember]
        public string Name
        {
            get { return this._name; }
            set
            {
                if (this._name != value)
                {
                    this._name = value;
                    if (!string.IsNullOrEmpty(this._name))
                        this._name = this._name.Split('.')[0];
                    OnPropertyChanged(nameof(this.Name));
                }
            }
        }
        /// <summary>True if the creation is private. False otherwise </summary>
        [DataMember]
        public bool IsPrivate { get; set; }
        /// <summary>Comments related to the creation </summary>
        public JArray Comments { get; set; }

        /// <summary>AWS S3 creation get url</summary>
        [DataMember]
        public string GetUrl { get; set; }
        [DataMember]
        private int _numberOfComment;

        /// <summary>Number of comments related to the creation</summary>
        public int NumberOfComment
        {
            get { return this._numberOfComment; }
            set
            {
                if (this._numberOfComment != value)
                {
                    this._numberOfComment = value;
                    OnPropertyChanged(nameof(NumberOfComment));
                }
            }
        }
        [DataMember]
        private int _numberOfLike;

        /// <summary>Number of likes related to the creation </summary>
        public int NumberOfLike
        {
            get { return this._numberOfLike; }
            set
            {
                if (this._numberOfLike != value)
                {
                    this._numberOfLike = value;
                    OnPropertyChanged(nameof(NumberOfLike));
                }
            }
        }
        /// <summary>List of the creators of the creation</summary>
        [DataMember]
        public List<UserBase> CreatorList { get; set; }
        public string Picture { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Creation()
        {
            this.Picture = "https://s3-eu-west-1.amazonaws.com/orphee/image/logo_transparent2.png";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
