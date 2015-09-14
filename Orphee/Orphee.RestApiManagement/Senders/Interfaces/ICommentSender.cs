using System.Threading.Tasks;

namespace Orphee.RestApiManagement.Senders.Interfaces
{
    public interface ICommentSender
    {
        Task<bool> SendComment(string comment, string creationId);
    }
}
