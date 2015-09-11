using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Orphee.RestApiManagement.Interfaces;

namespace Orphee.RestApiManagement
{
    public class ConnectionManager : IConnectionManager
    {
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
