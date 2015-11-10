using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Orphee.RestApiManagement.Models;
using Orphee.RestApiManagement.Posters.Interfaces;

namespace Orphee.RestApiManagement.Posters
{
    public class PasswordReseter : IPasswordReseter
    {
        public async Task<bool> ResetPassword(string actualPassword, string newPassword)
        {
            var values = new Dictionary<string, string>
            {

            };

            using (var httpClient = new HttpClient { BaseAddress = RestApiManagerBase.Instance.RestApiUrl })
            {
                using (var content = new FormUrlEncodedContent(values))
                using (var response = await httpClient.PostAsync(RestApiManagerBase.Instance.RestApiPath["reset password"], content))
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                        return false;
                    RestApiManagerBase.Instance.UserData = JsonConvert.DeserializeObject<UserData>(result);
                    RestApiManagerBase.Instance.IsConnected = true;
                }
                return true;
            }
        }
    }
}
