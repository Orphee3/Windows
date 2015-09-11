using System.Threading.Tasks;

namespace Orphee.RestApiManagement.Interfaces
{
    public interface ICommentSender
    {
        Task<bool> SendComment(string comment, string creationId);
    }
}
