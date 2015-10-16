using System.Threading.Tasks;

namespace Orphee.RestApiManagement.Senders.Interfaces
{
    /// <summary>
    /// NotifacationSender interface
    /// </summary>
    public interface INotificationSender
    {
        /// <summary>
        /// Sends a notification to the remote server
        /// </summary>
        /// <param name="notificationType">Represents the type of the notification</param>
        /// <param name="creationId">Target creation id</param>
        /// <returns>Returns true if the request was sent and the response received correctly. Returns false otherwise</returns>
        Task<bool> SendNotification(string notificationType, string creationId);
    }
}
