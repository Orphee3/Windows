using System.Threading.Tasks;

namespace Orphee.RestApiManagement.Interfaces
{
    public interface IFriendListGetter
    {
        Task<bool> RetrieveFriends();
    }
}
