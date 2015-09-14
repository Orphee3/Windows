using System.Threading.Tasks;

namespace Orphee.RestApiManagement.Getters.Interfaces
{
    public interface IUserFluxGetter
    {
        Task<bool> GetUserFlux();
    }
}
