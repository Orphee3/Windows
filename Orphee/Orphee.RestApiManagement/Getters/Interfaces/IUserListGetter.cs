using System.Collections.Generic;
using System.Threading.Tasks;
using Orphee.RestApiManagement.Models;

namespace Orphee.RestApiManagement.Getters.Interfaces
{
    public interface IUserListGetter
    {
        Task<List<User>> GetUserList(int offset, int size);
    }
}
