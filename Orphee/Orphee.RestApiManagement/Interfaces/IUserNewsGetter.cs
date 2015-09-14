using System.Threading.Tasks;

namespace Orphee.RestApiManagement.Interfaces
{
    public interface IUserNewsGetter
    {
        Task<object> GetUserNews();
    }
}
