using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Newtonsoft.Json.Linq;
using Orphee.Models;
using Orphee.RestApiManagement.Annotations;
using Orphee.RestApiManagement.Models.Interfaces;

namespace Orphee.RestApiManagement.Models
{
    [DataContract]
    public class UserBase : IUser, INotifyPropertyChanged
    {
        [DataMember]
        public string Id { get; set; }

        /// <summary>User UserName </summary>
        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        /// <summary>User name </summary>
        public string Name { get; set; }

        [DataMember]
        public List<string> Friends { get; set; }

        private string _picture;

        [DataMember]
        public string Picture
        {
            get { return this._picture; }
            set
            {
                if (this._picture == value)
                    return;
                this._picture = value;
                OnPropertyChanged(nameof(Picture));
            }
        }

        /// <summary>User creation date  </summary>
        public DateTime DateCreaion { get; set; }
        /// <summary>User comment list </summary>
        public JArray Comments { get; set; }
        /// <summary>User like list </summary>
        [DataMember]
        public List<string> Likes { get; set; }
        /// <summary>User creation list </summary>
        [DataMember]
        public List<string> Creations { get; set; }
        /// <summary>List of pending friend asking </summary>
        public Visibility AddButtonVisibility { get; set; }
        public bool IsChecked { get; set; }

        private SolidColorBrush _pictureDominantColor;
        public SolidColorBrush PictureDominantColor
        {
            get { return this._pictureDominantColor; }
            set
            {
                if (this._pictureDominantColor != value)
                {
                    this._pictureDominantColor = value;
                    OnPropertyChanged(nameof(PictureDominantColor));
                }
            }
        }
        protected AverageColorFinder _averageColorFinder;

        public UserBase()
        {
            if (string.IsNullOrEmpty(this.Picture))
                this.Picture = "ms-appx:///Assets/defaultUser.png";
        }

        public async void GetUserPictureDominantColor()
        {
            if (this._averageColorFinder == null)
                this._averageColorFinder = new AverageColorFinder();
            if (this.PictureDominantColor == null)
                this.PictureDominantColor = new SolidColorBrush();
            this.PictureDominantColor.Color = await this._averageColorFinder.GetDominantImageColor();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
