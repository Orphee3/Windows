using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Orphee.RestApiManagement.Models;
using Orphee.RestApiManagement.Senders.Interfaces;

namespace Orphee.RestApiManagement.Senders
{
    /// <summary>
    /// Class sending "like" notifications to the 
    /// remote server
    /// </summary>
    public class LikeSender : ILikeSender
    {
        private readonly INotificationSender _notifyer;

        /// <summary>
        /// Constructor initializing notifyer through 
        /// dependency injection
        /// </summary>
        /// <param name="notifyer">Instance of the NotificationSender class used to send notifications to the remote server</param>
        public LikeSender(INotificationSender notifyer)
        {
            this._notifyer = notifyer;
        }

        /// <summary>
        /// Sends a like notification to the server
        /// </summary>
        /// <param name="creationId">Target of the creation to be liked</param>
        /// <returns>Returns true if the request was sent and the response received correctly. Returns false otherwise</returns>
        public async Task<bool> SendLike(string creationId)
        {
            var values = new Dictionary<string, string>()
            {
                {"creator", RestApiManagerBase.Instance.UserData.User.Id},
                {"creation", creationId},
                {"message", creationId},
                {"parentId", creationId }
            };

            using (var httpClient = new HttpClient { BaseAddress = RestApiManagerBase.Instance.RestApiUrl })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authorization", "Bearer " + RestApiManagerBase.Instance.UserData.Token);
                using (var content = new FormUrlEncodedContent(values))
                {
                    using (var response = await httpClient.PostAsync(RestApiManagerBase.Instance.RestApiPath["like"], content))
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode)
                            return false;
                        var result = await this._notifyer.SendNotification("likes", creationId);
                        if (!result)
                            return false;
                    }
                }
                return true;
            }
        }
    }
}
