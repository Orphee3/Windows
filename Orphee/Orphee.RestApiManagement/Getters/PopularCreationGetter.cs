using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Models;

namespace Orphee.RestApiManagement.Getters
{
    public class PopularCreationGetter : IPopularCreationGetter
    {
        public async Task<List<Creation>> GetpopularCreation()
        {
            List<Creation> creationList;
            using (var httpClient = new HttpClient() { BaseAddress = RestApiManagerBase.Instance.RestApiUrl })
            {
                using (var response = await httpClient.GetAsync(RestApiManagerBase.Instance.RestApiPath["popular"]))
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                        return null;
                    creationList = JsonConvert.DeserializeObject<List<Creation>>(responseData);
                }
            }
            return creationList;
        }
    }
}
