using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orphee.RestApiManagement.Interfaces
{
    public interface IRoomMessageListGetter
    {
        Task<List<Message>> GetRoomMessageList(string roomId);
    }
}
