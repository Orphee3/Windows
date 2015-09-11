using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Orphee.RestApiManagement.Interfaces;

namespace Orphee.RestApiManagement
{
    public class UserFriendListGetter : IUserFriendListGetter
    {
        public async Task<List<User>> GetUserFriendList()
        {
            List<User> friendList;
            using (var httpClient = new HttpClient { BaseAddress = RestApiManagerBase.Instance.RestApiUrl })
            {
                using (var response = await httpClient.GetAsync(RestApiManagerBase.Instance.RestApiPath["users"] + "/" + RestApiManagerBase.Instance.UserData.User.Id + "/friends"))
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                        return null;
                    friendList = JsonConvert.DeserializeObject<List<User>>(result);
                }
            }
            return friendList;
        }
    }
}
