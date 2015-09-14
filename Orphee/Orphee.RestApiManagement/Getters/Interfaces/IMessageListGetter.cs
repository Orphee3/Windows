using System.Collections.Generic;
using System.Threading.Tasks;
using Orphee.RestApiManagement.Models;

namespace Orphee.RestApiManagement.Getters.Interfaces
{
    public interface IMessageListGetter
    {
        Task<List<Message>> GetRoomMessageList(string roomId);
    }
}
