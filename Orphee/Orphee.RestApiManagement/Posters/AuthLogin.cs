using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Orphee.RestApiManagement.Models;
using Orphee.RestApiManagement.Posters.Interfaces;

namespace Orphee.RestApiManagement.Posters
{
    public class AuthLogin : IAuthLogin
    {
        public async Task<bool> ConnectThroughSDK(string provider, string code, string clientId, string redirectUrl)
        {
            var request = provider == "facebook" ? RestApiManagerBase.Instance.RestApiPath["facebook"] : RestApiManagerBase.Instance.RestApiPath["google"];
            var values = new Dictionary<string, string>
            {
                {"clientId", clientId},
                {"code", code},
                {"redirectUri", redirectUrl}
            };

            using (var httpClient = new HttpClient { BaseAddress = RestApiManagerBase.Instance.RestApiUrl })
            {
                using (var content = new FormUrlEncodedContent(values))
                using (var response = await httpClient.PostAsync(request, content))
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                        return false;
                    RestApiManagerBase.Instance.UserData = JsonConvert.DeserializeObject<UserData>(result);
                    RestApiManagerBase.Instance.IsConnected = true;
                    RestApiManagerBase.Instance.UserData.User.GetUserPictureDominantColor();
                }
                return true;
            }
        }
    }
}
