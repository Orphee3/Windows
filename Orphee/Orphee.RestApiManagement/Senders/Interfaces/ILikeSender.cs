using System.Threading.Tasks;

namespace Orphee.RestApiManagement.Senders.Interfaces
{
    public interface ILikeSender
    {
        Task<bool> SendLike(string creationId);
    }
}
