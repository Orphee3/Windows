using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Orphee.RestApiManagement.Interfaces
{
    public interface IUserCreationGetter
    {
        Task<List<Creation>> GetUserCreations(string userId);
    }
}
