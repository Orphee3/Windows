using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Json;
using Windows.Storage;
using Orphee.RestApiManagement.Annotations;
using Orphee.RestApiManagement.Models.Interfaces;

namespace Orphee.RestApiManagement.Models
{
    /// <summary>
    /// Singleton class managing all the API routes and the
    /// currently logged user
    /// </summary>
    public class RestApiManagerBase : IRestApiManagerBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>RestApiManagerBase instance</summary>
        public static RestApiManagerBase Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new RestApiManagerBase();
                return _instance;
            }
        }
        private static RestApiManagerBase _instance;
        /// <summary>Server's url </summary>
        public Uri RestApiUrl { get; private set; }
        /// <summary>NotificationReceiver</summary>
        /// <summary>Contains all the routes </summary>
        public Dictionary<string, string> RestApiPath { get; private set; }
        /// <summary>Current logged user </summary>
        public IUserData UserData { get; set; }

        private bool _isConnected;
        /// <summary>True if the user is logged. False otherwise </summary>
        public bool IsConnected
        {
            get { return this._isConnected; }
            set
            {
                if (this._isConnected != value)
                {
                    this._isConnected = value;
                    OnPropertyChanged(nameof(IsConnected));
                }
            }
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        public RestApiManagerBase()
        {
            this.RestApiUrl = new Uri("http://163.5.84.242:3000/");
            InitializeRestApiPath();
        }

        private void InitializeRestApiPath()
        {
            this.RestApiPath = new Dictionary<string, string>
            {
                {"register", "api/register"},
                {"login", "api/login"},
                {"users", "api/user"},
                {"askfriend", "api/askfriend"},
                {"creation", "api/creation"},
                {"comment", "api/comment"},
                {"acceptfriend", "api/acceptfriend"},
                {"like", "api/like/"},
                {"dislike", "api/dislike/"},
                {"notify", "api/notify/"},
                {"popular", "api/creationPopular"},
                {"private room", "api/room/privateMessage/" },
                {"group room", "api/room/" },
                {"forgot", "api/forgot" },
                {"google", "auth/google" },
                {"facebook", "auth/facebook" },
                {"remove friend", "api/removeFriend/" },
            };
        }

        /// <summary>
        /// Logs out the current user
        /// </summary>
        public void Logout()
        {
            RemoveUnimportantData();
            SaveUser();
            this.IsConnected = false;
        }

        public async void SaveUser()
        {
            var serializer = new DataContractJsonSerializer(typeof(ObservableCollection<UserData>));
            try
            {
                using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync("User-" + this.UserData.User.UserName + ".json", CreationCollisionOption.ReplaceExisting))
                {
                    serializer.WriteObject(stream, this.UserData);
                }
            }
            catch (Exception)
            {

            }
        }

        private void RemoveUnimportantData()
        {
            this.UserData.Token = null;
            this.UserData.User.NotificationList.Clear();
            this.UserData.User.ConversationList.Clear();
            this.UserData.User.Friends = null;
        }

        public async void RetreiveUser()
        {
            var jsonSerializer = new DataContractJsonSerializer(typeof(ObservableCollection<UserData>));
            var files = await ApplicationData.Current.LocalFolder.GetFilesAsync();
            var userFiles = files.Where(f => f.Name.Contains("User")).ToList();
            try
            {
                foreach (var tmpUser in userFiles.Select(file => ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(file.Name).Result).Select(myStream => (UserData)jsonSerializer.ReadObject(myStream)).Where(tmpUser => !string.IsNullOrEmpty(tmpUser.Token)))
                {
                    this.UserData = tmpUser;
                    this.IsConnected = true;
                    this.UserData.User.GetUserPictureDominantColor();
                    return;
                }
            }
            catch (Exception)
            {

            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
