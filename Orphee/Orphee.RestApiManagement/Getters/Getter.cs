using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Models;

namespace Orphee.RestApiManagement.Getters
{
    /// <summary>
    /// Class managing all the 
    /// Get request of the application to the remote server
    /// </summary>
    public class Getter : IGetter
    {
        /// <summary>
        /// Gets the asked data from the remote server
        /// </summary>
        /// <typeparam name="T">Type of the requested data</typeparam>
        /// <param name="request">Request that's going to be sent to the server</param>
        /// <returns>Return the type T data retreived from the server</returns>
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
