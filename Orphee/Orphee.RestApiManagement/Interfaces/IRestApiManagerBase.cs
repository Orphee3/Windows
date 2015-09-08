using System;
using System.Collections.Generic;

namespace Orphee.RestApiManagement.Interfaces
{
    public interface IRestApiManagerBase
    {
        Uri RestApiUrl { get; }
        List<User> UserNameList { get; set; }
        Dictionary<string, string> RestApiPath { get; }
        IUserData UserData { get; set; }
        bool IsConnected { get; set; }
        void Logout();
    }
}
