using System.Threading.Tasks;

namespace Orphee.RestApiManagement.Interfaces
{
    public interface IFriendAccepter
    {
        Task<bool> AcceptFriend(string friendId);
    }
}
