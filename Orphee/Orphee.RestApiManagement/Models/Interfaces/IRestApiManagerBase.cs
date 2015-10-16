using System;
using System.Collections.Generic;

namespace Orphee.RestApiManagement.Models.Interfaces
{
    /// <summary>
    /// RestApiManagerBase interface
    /// </summary>
    public interface IRestApiManagerBase
    {
        /// <summary>Server's url </summary>
        Uri RestApiUrl { get; }
        /// <summary>Contains all the routes </summary>
        Dictionary<string, string> RestApiPath { get; }
        /// <summary>Current logged user </summary>
        IUserData UserData { get; set; }
        /// <summary>True if the user is logged. False otherwise </summary>
        bool IsConnected { get; set; }
        
        /// <summary>
        /// Logs out the current user
        /// </summary>
        void Logout();
    }
}
