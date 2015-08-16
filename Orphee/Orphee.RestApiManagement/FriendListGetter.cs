using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Orphee.RestApiManagement.Interfaces;

namespace Orphee.RestApiManagement
{
    public class FriendListGetter : IFriendListGetter
    {
        public async Task<bool> RetrieveFriends()
        {
            using (var httpClient = new HttpClient { BaseAddress = RestApiManagerBase.Instance.RestApiUrl })
            {
                using (var response = await httpClient.GetAsync("api/user/" + RestApiManagerBase.Instance.UserData.User.Id + "/friends"))
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                        return false;
                   // RestApiManagerBase.Instance.UserData.User.Friends = JsonConvert.DeserializeObject<List<User>>(responseData);
                }
            }
            return true;
        } 
    }
}
