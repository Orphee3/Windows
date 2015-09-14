using Orphee.RestApiManagement.Models.Interfaces;

namespace Orphee.RestApiManagement.Models
{
    public class UserData : IUserData
    {
        public string Token { get; set; }
        public User User { get; set; }
    }
}
