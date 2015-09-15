using System;
using System.Collections.Generic;
using Orphee.RestApiManagement.Interfaces;
using Orphee.RestApiManagement.Models.Interfaces;
using Orphee.RestApiManagement.Socket_Management;

namespace Orphee.RestApiManagement.Models
{
    public class RestApiManagerBase : IRestApiManagerBase
    {
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
        public Uri RestApiUrl { get; private set; }
        public NotificationRecieiver NotificationRecieiver {get; private set; }
        public Dictionary<string, string> RestApiPath { get; private set; }
        public IUserData UserData { get; set; }

        public bool IsConnected { get; set; }

        public RestApiManagerBase()
        {
            this.IsConnected = false;
            this.RestApiUrl = new Uri("http://163.5.84.242:3000/");
            this.NotificationRecieiver = new NotificationRecieiver();
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
                {"like", "api/like"},
                {"roomMessages", "api/room/privateMessage"},
                {"notify", "api/notify/"},
                {"popular", "api/creationPopular" },
            };
        }

        private void RetrieveUserNamesFromCacheMemory()
        {

        }

        public void Logout()
        {
            this.UserData = null;
            this.IsConnected = false;
            this.NotificationRecieiver.CloseSocket();
        }
    }
}
