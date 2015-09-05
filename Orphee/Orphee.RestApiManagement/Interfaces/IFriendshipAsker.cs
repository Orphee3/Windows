using System.Threading.Tasks;

namespace Orphee.RestApiManagement.Interfaces
{
    public interface IFriendshipAsker
    {
        Task<bool> SendFriendshipRequestToRestApi(string friendId);
    }
}
