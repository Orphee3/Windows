using System;
using System.Collections.Generic;
using Orphee.RestApiManagement.Models.Interfaces;
using Orphee.RestApiManagement.Socket_Management;

namespace Orphee.RestApiManagement.Models
{
    /// <summary>
    /// Singleton class managing all the API routes and the
    /// currently logged user
    /// </summary>
    public class RestApiManagerBase : IRestApiManagerBase
    {
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
        public NotificationRecieiver NotificationRecieiver {get; private set; }
        /// <summary>Contains all the routes </summary>
        public Dictionary<string, string> RestApiPath { get; private set; }
        /// <summary>Current logged user </summary>
        public IUserData UserData { get; set; }
        /// <summary>True if the user is logged. False otherwise </summary>
        public bool IsConnected { get; set; }
        
        /// <summary>
        /// Constructor
        /// </summary>
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
                {"popular", "api/creationPopular"},
            };
        }

        /// <summary>
        /// Logs out the current user
        /// </summary>
        public void Logout()
        {
            this.UserData = null;
            this.IsConnected = false;
            this.NotificationRecieiver.CloseSocket();
        }
    }
}
