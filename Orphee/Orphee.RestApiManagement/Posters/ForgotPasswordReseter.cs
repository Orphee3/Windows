using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Data.Json;
using Newtonsoft.Json.Linq;
using Orphee.RestApiManagement.Models;
using Orphee.RestApiManagement.Posters.Interfaces;

namespace Orphee.RestApiManagement.Posters
{
    public class ForgotPasswordReseter : IForgotPasswordReseter
    {
        public async Task<bool> ResetForgotPassword(string mailAdress)
        {
            var values = new Dictionary<string, string>
            {
                { "username", mailAdress },
            };
            using (var httpClient = new HttpClient { BaseAddress = RestApiManagerBase.Instance.RestApiUrl })
            {
                using (var content = new FormUrlEncodedContent(values))
                using (var response = await httpClient.PostAsync(RestApiManagerBase.Instance.RestApiPath["forgot"], content))
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                        return false;
                }
                return true;
            }
        }
    }
}
