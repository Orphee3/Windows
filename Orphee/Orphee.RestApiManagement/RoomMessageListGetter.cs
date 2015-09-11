using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Orphee.RestApiManagement.Interfaces;

namespace Orphee.RestApiManagement
{
    public class RoomMessageListGetter : IRoomMessageListGetter
    {
        public async Task<List<Message>> GetRoomMessageList(string userId)
        {
            List<Message> messageList;
            using (var httpClient = new HttpClient() { BaseAddress = RestApiManagerBase.Instance.RestApiUrl })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authorization", "Bearer " + RestApiManagerBase.Instance.UserData.Token);
                using (var response = await httpClient.GetAsync(RestApiManagerBase.Instance.RestApiPath["roomMessages"] + "/" + userId))
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                        return null;
                    messageList = JsonConvert.DeserializeObject<List<Message>>(responseData);
                    messageList.Reverse();
                }
            }
            return messageList;
        }
    }
}
