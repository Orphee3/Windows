using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orphee.RestApiManagement.Interfaces
{
    public interface IUserListGetter
    {
        Task<List<User>> GetUserList(int offset, int size);
    }
}
