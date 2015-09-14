using System.Threading.Tasks;

namespace Orphee.RestApiManagement.Getters.Interfaces
{
    public interface IUserNewsGetter
    {
        Task<object> GetUserNews();
    }
}
