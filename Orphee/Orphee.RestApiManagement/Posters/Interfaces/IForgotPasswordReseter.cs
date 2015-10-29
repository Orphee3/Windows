using System.Threading.Tasks;

namespace Orphee.RestApiManagement.Posters.Interfaces
{
    public interface IForgotPasswordReseter
    {
        Task<bool> ResetForgotPassword(string mailAdress);
    }
}
