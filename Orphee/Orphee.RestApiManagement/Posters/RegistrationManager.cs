using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Orphee.RestApiManagement.Models;
using Orphee.RestApiManagement.Posters.Interfaces;

namespace Orphee.RestApiManagement.Posters
{
    /// <summary>
    /// Class managing the user's registration in
    /// order to create a new account
    /// </summary>
    public class RegistrationManager : IRegistrationManager
    {
        /// <summary>
        /// Sends the user's registrations data in order
        /// to create a new account
        /// </summary>
        /// <param name="userName">Username of the user</param>
        /// <param name="name">Name of the user</param>
        /// <param name="password">Password of the accout</param>
        /// <returns>Return true if the account was created. Return false otherwise</returns>
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
                    RestApiManagerBase.Instance.NotificationRecieiver.InitSocket();
                }
                return true;
            }
        }
    }
}
