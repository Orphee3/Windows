﻿using System.Net.Http;
using System.Threading.Tasks;
using Orphee.RestApiManagement.Interfaces;

namespace Orphee.RestApiManagement
{
    public class UserFluxGetter : IUserFluxGetter
    {
        public async Task<bool> GetUserFlux()
        {
            using (var httpClient = new HttpClient { BaseAddress = RestApiManagerBase.Instance.RestApiUrl})
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authorization", "Bearer " + RestApiManagerBase.Instance.UserData.Token);
                using (var response = await httpClient.GetAsync(RestApiManagerBase.Instance.RestApiPath["users"] + "/" + RestApiManagerBase.Instance.UserData.User.Id + "/flux"))
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                        return false;
                }
                return true;
            }
        }
    }
}
