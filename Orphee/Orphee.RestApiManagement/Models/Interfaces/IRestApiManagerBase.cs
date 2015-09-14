using System;
using System.Collections.Generic;

namespace Orphee.RestApiManagement.Models.Interfaces
{
    public interface IRestApiManagerBase
    {
        Uri RestApiUrl { get; }
        Dictionary<string, string> RestApiPath { get; }
        IUserData UserData { get; set; }
        bool IsConnected { get; set; }
        void Logout();
    }
}
