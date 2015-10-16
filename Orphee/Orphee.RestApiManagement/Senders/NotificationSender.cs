using System.Net.Http;
using System.Threading.Tasks;
using Orphee.RestApiManagement.Models;
using Orphee.RestApiManagement.Senders.Interfaces;

namespace Orphee.RestApiManagement.Senders
{
    /// <summary>
    /// Class managing the sending of notifications to the
    /// remote server
    /// </summary>
    public class NotificationSender : INotificationSender
    {
        /// <summary>
        /// Sends a notification to the remote server
        /// </summary>
        /// <param name="notificationType">Represents the type of the notification</param>
        /// <param name="creationId">Target creation id</param>
        /// <returns>Returns true if the request was sent and the response received correctly. Returns false otherwise</returns>
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
