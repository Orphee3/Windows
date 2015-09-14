using System.Threading.Tasks;

namespace Orphee.RestApiManagement.Getters.Interfaces
{
    public interface IFriendListGetter
    {
        Task<bool> RetrieveFriends();
    }
}
