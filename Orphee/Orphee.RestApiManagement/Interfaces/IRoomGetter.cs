using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orphee.RestApiManagement.Interfaces
{
    public interface IRoomGetter
    {
        Task<List<Conversation>> GetUserRooms();
    }
}
