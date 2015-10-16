using System.Threading.Tasks;

namespace Orphee.RestApiManagement.Senders.Interfaces
{
    /// <summary>
    /// CommentSender interface
    /// </summary>
    public interface ICommentSender
    {
        /// <summary>
        /// Sends a comment to the remote server
        /// </summary>
        /// <param name="comment">Comment to send</param>
        /// <param name="creationId">Target creation id</param>
        /// <returns>Returns true if the request was sent and the response received correctly. Returns false otherwise</returns>
        Task<bool> SendComment(string comment, string creationId);
    }
}
