using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Models;

namespace Orphee.RestApiManagement.Getters
{
    public class Getter : IGetter
    {
        public async Task<T> GetInfo<T>(string request)
        {
            var returnValue = default(T);
            using (var httpClient = new HttpClient {BaseAddress = RestApiManagerBase.Instance.RestApiUrl})
            {
                if (RestApiManagerBase.Instance.IsConnected)
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authorization", "Bearer " + RestApiManagerBase.Instance.UserData.Token);
                using (var response = await httpClient.GetAsync(request))
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                        return returnValue;
                    returnValue = JsonConvert.DeserializeObject<T>(result);
                }
            }
            return returnValue;
        } 
    }
}
