using System.Threading.Tasks;

namespace Orphee.RestApiManagement.Senders.Interfaces
{
    public interface INotificationSender
    {
        Task<bool> SendNotification(string notificationType, string creationId);
    }
}
