using System.Threading.Tasks;

namespace Orphee.RestApiManagement.Interfaces
{
    public interface IConnectionManager
    {
        Task<bool> ConnectUser(string userName, string password);
    }
}
