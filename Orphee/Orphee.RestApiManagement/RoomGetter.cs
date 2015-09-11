using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Orphee.RestApiManagement.Interfaces;

namespace Orphee.RestApiManagement
{
    public class RoomGetter : IRoomGetter
    {
        public async Task<List<Conversation>> GetUserRooms()
        {
            List<Conversation> roomList;
            using (var httpClient = new HttpClient() {BaseAddress = RestApiManagerBase.Instance.RestApiUrl})
            {
                using (var response = await httpClient.GetAsync(RestApiManagerBase.Instance.RestApiPath["users"] + "/" + RestApiManagerBase.Instance.UserData.User.Id + "/rooms"))
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                        return null;
                    roomList = JsonConvert.DeserializeObject<List<Conversation>>(responseData);
                }
            }
            return roomList;
        }
    }
}
