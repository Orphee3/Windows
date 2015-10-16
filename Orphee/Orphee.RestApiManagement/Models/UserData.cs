using Orphee.RestApiManagement.Models.Interfaces;

namespace Orphee.RestApiManagement.Models
{
    /// <summary>
    /// Class respecting the JSON userData representation
    /// given by the server
    /// </summary>
    public class UserData : IUserData
    {
        /// <summary>User's connexion token </summary>
        public string Token { get; set; }
        /// <summary>User's representation containing all its related data</summary>
        public User User { get; set; }
    }
}
