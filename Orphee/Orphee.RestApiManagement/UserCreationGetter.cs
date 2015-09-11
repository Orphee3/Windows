using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Orphee.RestApiManagement.Interfaces;

namespace Orphee.RestApiManagement
{
    public class UserCreationGetter : IUserCreationGetter
    {
        public async Task<List<Creation>> GetUserCreations(string userId)
        {
            List<Creation> userCreations = null;
            using (var httpClient = new HttpClient { BaseAddress = RestApiManagerBase.Instance.RestApiUrl })
            {
                using (var response = await httpClient.GetAsync(RestApiManagerBase.Instance.RestApiPath["users"] + "/" + userId + "/creation"))
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK || result == "")
                        return null;
                    userCreations  = JsonConvert.DeserializeObject<List<Creation>>(result);
                }
            }
            return userCreations;
        }
    }
}
