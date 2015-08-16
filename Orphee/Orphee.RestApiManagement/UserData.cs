using Orphee.RestApiManagement.Interfaces;

namespace Orphee.RestApiManagement
{
    public class UserData : IUserData
    {
        public string Token { get; set; }
        public User User { get; set; }
    }
}
