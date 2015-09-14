using System.Net.Http;
using System.Threading.Tasks;
using Orphee.RestApiManagement.Models;
using Orphee.RestApiManagement.Senders.Interfaces;

namespace Orphee.RestApiManagement.Senders
{
    public class NotificationSender : INotificationSender
    {
        public async Task<bool> SendNotification(string notificationType, string creationId)
        {
            using (var httpClient = new HttpClient { BaseAddress = RestApiManagerBase.Instance.RestApiUrl })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authorization", "Bearer " + RestApiManagerBase.Instance.UserData.Token);
                using (var response = await httpClient.GetAsync(RestApiManagerBase.Instance.RestApiPath["notify"] + notificationType + "/" + creationId))
                {
                    if (!response.IsSuccessStatusCode)
                        return false;
                }
                return true;
            }
        }
    }
}
