using System.Threading.Tasks;

namespace Orphee.RestApiManagement.Interfaces
{
    public interface ILikeSender
    {
        Task<bool> SendLike(string creationId);
    }
}
