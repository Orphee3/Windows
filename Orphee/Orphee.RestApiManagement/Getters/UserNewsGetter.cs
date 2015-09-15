using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Models;

namespace Orphee.RestApiManagement.Getters
{
    public class UserNewsGetter : IUserNewsGetter
    {
        public async Task<List<News>> GetUserNews()
        {
            List<News> newsList;
            using (var httpClient = new HttpClient { BaseAddress = RestApiManagerBase.Instance.RestApiUrl })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authorization", "Bearer " + RestApiManagerBase.Instance.UserData.Token);
                using (var response = await httpClient.GetAsync(RestApiManagerBase.Instance.RestApiPath["users"] + "/" + RestApiManagerBase.Instance.UserData.User.Id + "/news"))
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                        return null;
                    newsList = JsonConvert.DeserializeObject<List<News>>(responseData);
                }
                return newsList;
            }
        }
    }
}
