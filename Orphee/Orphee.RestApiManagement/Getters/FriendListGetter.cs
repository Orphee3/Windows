﻿using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Models;

namespace Orphee.RestApiManagement.Getters
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
                    RestApiManagerBase.Instance.UserData.User.Friends = JsonConvert.DeserializeObject<JArray>(responseData);
                }
            }
            return true;
        } 
    }
}