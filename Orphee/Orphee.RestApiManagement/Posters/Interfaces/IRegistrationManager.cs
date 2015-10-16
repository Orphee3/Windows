using System.Threading.Tasks;

namespace Orphee.RestApiManagement.Posters.Interfaces
{
    /// <summary>
    /// RegistrationManager interface
    /// </summary>
    public interface IRegistrationManager
    {
        /// <summary>
        /// Sends the user's registrations data in order
        /// to create a new account
        /// </summary>
        /// <param name="userName">Username of the user</param>
        /// <param name="name">Name of the user</param>
        /// <param name="password">Password of the accout</param>
        /// <returns>Return true if the account was created. Return false otherwise</returns>
        Task<bool> RegisterUser(string userName, string name, string password);
    }
}
