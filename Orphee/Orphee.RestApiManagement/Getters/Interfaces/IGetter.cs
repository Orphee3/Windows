using System.Threading.Tasks;

namespace Orphee.RestApiManagement.Getters.Interfaces
{
    /// <summary>
    /// Getter intterface
    /// </summary>
    public interface IGetter
    {
        /// <summary>
        /// Gets the asked data from the remote server
        /// </summary>
        /// <typeparam name="T">Type of the requested data</typeparam>
        /// <param name="request">Request that's going to be sent to the server</param>
        /// <returns>Return the type T data retreived from the server</returns>
        Task<T> GetInfo<T>(string request);
    }
}
