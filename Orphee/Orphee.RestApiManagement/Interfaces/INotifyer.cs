using System.Threading.Tasks;

namespace Orphee.RestApiManagement.Interfaces
{
    public interface INotifyer
    {
        Task<bool> SendNotification(string notificationType, string creationId);
    }
}
