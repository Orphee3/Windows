using System.Threading.Tasks;

namespace Orphee.RestApiManagement.Getters.Interfaces
{
    public interface IGetter
    {
        Task<T> GetInfo<T>(string request);
    }
}
