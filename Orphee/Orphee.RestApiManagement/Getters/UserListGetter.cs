using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Models;

namespace Orphee.RestApiManagement.Getters
{
    public class UserListGetter : IUserListGetter
    {
        public async Task<List<User>> GetUserList(int offset, int size)
        {
            List<User> userList;
            using (var httpClient = new HttpClient {BaseAddress = RestApiManagerBase.Instance.RestApiUrl})
            {
                    using (var response = await httpClient.GetAsync(RestApiManagerBase.Instance.RestApiPath["users"] + "?offset=" + offset + "&size=" + size))
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        if (response.StatusCode != HttpStatusCode.OK)
                            return null;
                        userList = JsonConvert.DeserializeObject<List<User>>(result);
                        if (RestApiManagerBase.Instance.IsConnected)
                            userList.Remove(userList.FirstOrDefault(u => u.Name == RestApiManagerBase.Instance.UserData.User.Name));
                    }
            }
            return userList;
        }
    }
}
