using System.Threading.Tasks;

namespace Orphee.RestApiManagement.Getters.Interfaces
{
    public interface IFriendshipAsker
    {
        Task<bool?> SendFriendshipRequestToRestApi(string friendId);
    }
}
