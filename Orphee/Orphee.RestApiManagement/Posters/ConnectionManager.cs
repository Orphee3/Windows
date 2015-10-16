using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Orphee.RestApiManagement.Models;
using Orphee.RestApiManagement.Posters.Interfaces;

namespace Orphee.RestApiManagement.Posters
{
    /// <summary>
    /// Class managing the login
    /// of the user to its account
    /// </summary>
    public class ConnectionManager : IConnectionManager
    {
        /// <summary>
        /// Sends the user's login data in order
        /// log to it's account
        /// </summary>
        /// <param name="userName">Username of the user</param>
        /// <param name="password">Password of the accout</param>
        /// <returns>Return true if the user was logged successfully. Return false otherwise</returns>
        public async Task<bool> ConnectUser(string userName, string password)
        {
            using (var httpClient = new HttpClient {BaseAddress = RestApiManagerBase.Instance.RestApiUrl})
            {
                var toEncodeAsBytes = Encoding.UTF8.GetBytes(userName + ":" + password);
                var base64EncodedString = Convert.ToBase64String(toEncodeAsBytes);

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authorization", "Bearer " + base64EncodedString);

                using (var content = new StringContent(""))
                {
                    using (var response = await httpClient.PostAsync(RestApiManagerBase.Instance.RestApiPath["login"], content))
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        if (response.StatusCode != HttpStatusCode.OK)
                            return false;
                        RestApiManagerBase.Instance.UserData = JsonConvert.DeserializeObject<UserData>(result);
                        RestApiManagerBase.Instance.IsConnected = true;
                        RestApiManagerBase.Instance.NotificationRecieiver.InitSocket();
                    }
                }
                return true;
            }
        }
    }
}
