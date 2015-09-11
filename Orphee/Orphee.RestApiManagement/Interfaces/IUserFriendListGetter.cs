using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orphee.RestApiManagement.Interfaces
{
    public interface IUserFriendListGetter
    {
        Task<List<User>> GetUserFriendList();
    }
}
