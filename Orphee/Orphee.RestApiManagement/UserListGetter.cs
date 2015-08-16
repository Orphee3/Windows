using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Orphee.RestApiManagement.Interfaces;

namespace Orphee.RestApiManagement
{
    public class UserListGetter : IUserListGetter
    {
        public async Task<List<User>> GetUserList()
        {
            var userList = new List<User>();
            using (var httpClient = new HttpClient {BaseAddress = RestApiManagerBase.Instance.RestApiUrl})
            {
                    using (var response = await httpClient.GetAsync(RestApiManagerBase.Instance.RestApiPath["users"]))
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        if (response.StatusCode != HttpStatusCode.OK)
                            return null;
                        userList = JsonConvert.DeserializeObject<List<User>>(result);
                        RestApiManagerBase.Instance.IsConnected = true;
                    }
            }
            return userList;
        }
    }
}
