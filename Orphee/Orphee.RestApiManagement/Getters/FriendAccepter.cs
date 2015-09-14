using System.Net.Http;
using System.Threading.Tasks;
using Orphee.RestApiManagement.Interfaces;
using Orphee.RestApiManagement.Models;

namespace Orphee.RestApiManagement.Getters
{
    public class FriendAccepter : IFriendAccepter
    {
        public async Task<bool> AcceptFriend(string friendId)
        {
            using (var httpClient = new HttpClient { BaseAddress = RestApiManagerBase.Instance.RestApiUrl })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authorization", "Bearer " + RestApiManagerBase.Instance.UserData.Token);
                using (var response = await httpClient.GetAsync(RestApiManagerBase.Instance.RestApiPath["acceptfriend"] + "/" + friendId))
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                        return false;
                }
            }
            return true;
        }
    }
}
