using System.Threading.Tasks;

namespace Orphee.RestApiManagement.Interfaces
{
    public interface IRegistrationManager
    {
        Task<bool> RegisterUser(string userName, string name, string password);
    }
}
