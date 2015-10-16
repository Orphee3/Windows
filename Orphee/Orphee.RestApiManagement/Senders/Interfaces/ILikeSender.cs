using System.Threading.Tasks;

namespace Orphee.RestApiManagement.Senders.Interfaces
{
    /// <summary>
    /// LikeSender interface
    /// </summary>
    public interface ILikeSender
    {
        /// <summary>
        /// Sends a like notification to the server
        /// </summary>
        /// <param name="creationId">Target of the creation to be liked</param>
        /// <returns>Returns true if the request was sent and the response received correctly. Returns false otherwise</returns>
        Task<bool> SendLike(string creationId);
    }
}
