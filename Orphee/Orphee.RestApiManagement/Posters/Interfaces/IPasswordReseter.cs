using System.Threading.Tasks;

namespace Orphee.RestApiManagement.Posters.Interfaces
{
    public interface IPasswordReseter
    {
        Task<bool> ResetPassword(string actualPassword, string newPassword);
    }
}
