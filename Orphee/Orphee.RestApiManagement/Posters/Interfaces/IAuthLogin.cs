using System.Threading.Tasks;

namespace Orphee.RestApiManagement.Posters.Interfaces
{
    public interface IAuthLogin
    {
        Task<bool> ConnectThroughSDK(string provider, string token, string clientId, string redirectUrl);
    }
}
