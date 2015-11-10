using System.Threading.Tasks;

namespace Orphee.Models.Interfaces
{
    public interface IOnUserLoginNewsGetter
    {
        Task<bool> GetUserNewsInformation();
    }
}
