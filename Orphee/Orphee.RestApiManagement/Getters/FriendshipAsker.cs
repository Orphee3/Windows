﻿using System.Net.Http;
using System.Threading.Tasks;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Interfaces;
using Orphee.RestApiManagement.Models;

namespace Orphee.RestApiManagement.Getters
{
    public class FriendshipAsker : IFriendshipAsker
    {
        public async Task<bool?> SendFriendshipRequestToRestApi(string friendId)
        {
            using (var httpClient = new HttpClient { BaseAddress = RestApiManagerBase.Instance.RestApiUrl })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authorization", "Bearer " + RestApiManagerBase.Instance.UserData.Token);
                using (var response = await httpClient.GetAsync(RestApiManagerBase.Instance.RestApiPath["askfriend"] + "/" + friendId))
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                        return null;

                    return response.ReasonPhrase == "OK";
                }
            }
        }
    }
}