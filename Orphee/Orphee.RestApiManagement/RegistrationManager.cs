﻿using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Orphee.RestApiManagement.Interfaces;

namespace Orphee.RestApiManagement
{
    public class RegistrationManager : IRegistrationManager
    {
        private readonly IUserListGetter _userListGetter;
        public RegistrationManager(IUserListGetter userListGetter)
        {
            this._userListGetter = userListGetter;
        }
        public async Task<bool> RegisterUser(string userName, string name, string password)
        {
            var values = new Dictionary<string, string>
            {
                { "name", name },
                { "username", userName },
                { "password", password}
            };

            using (var httpClient = new HttpClient { BaseAddress = RestApiManagerBase.Instance.RestApiUrl })
            {
                using (var content = new FormUrlEncodedContent(values))
                using (var response = await httpClient.PostAsync(RestApiManagerBase.Instance.RestApiPath["register"], content))
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                        return false;
                    RestApiManagerBase.Instance.UserData = JsonConvert.DeserializeObject<UserData>(result);
                    RestApiManagerBase.Instance.IsConnected = true;
                    RestApiManagerBase.Instance.NotificationRecieiver.Run();
                    RestApiManagerBase.Instance.UserNameList = await this._userListGetter.GetUserList(0, 30);
                }
                return true;
            }
        }
    }
}
