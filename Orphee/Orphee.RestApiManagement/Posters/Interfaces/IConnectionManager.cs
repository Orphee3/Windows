using System.Threading.Tasks;

namespace Orphee.RestApiManagement.Posters.Interfaces
{
    /// <summary>
    /// ConnectionManager interface
    /// </summary>
    public interface IConnectionManager
    {
        /// <summary>
        /// Sends the user's login data in order
        /// log to it's account
        /// </summary>
        /// <param name="userName">Username of the user</param>
        /// <param name="password">Password of the accout</param>
        /// <returns>Return true if the user was logged successfully. Return false otherwise</returns>
        Task<bool> ConnectUser(string userName, string password);
    }
}
