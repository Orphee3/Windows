using System;
using System.Collections.Generic;
using Orphee.RestApiManagement.Interfaces;

namespace Orphee.RestApiManagement
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
        public List<string> UserNameList { get; private set; }
        public Dictionary<string, string> RestApiPath { get; private set; }
        public IUserData UserData { get; set; }
        public bool IsConnected { get; set; }

        public RestApiManagerBase()
        {
            this.IsConnected = false;
            this.RestApiUrl = new Uri("http://163.5.84.242:3000/");
            InitializeRestApiPath();
        }

        private void InitializeRestApiPath()
        {
            this.RestApiPath = new Dictionary<string, string>();
            this.RestApiPath.Add("register", "api/register");
            this.RestApiPath.Add("login", "api/login");
            this.RestApiPath.Add("users", "api/user?offset=7&size=15");
        }

        private void RetrieveUserNamesFromCacheMemory()
        {

        }
    }
}
