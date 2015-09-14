using System.Threading.Tasks;

namespace Orphee.RestApiManagement.Posters.Interfaces
{
    public interface IConnectionManager
    {
        Task<bool> ConnectUser(string userName, string password);
    }
}
