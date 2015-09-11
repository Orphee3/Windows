using System.Threading.Tasks;

namespace Orphee.RestApiManagement.Interfaces
{
    public interface IUserFluxGetter
    {
        Task<bool> GetUserFlux();
    }
}
