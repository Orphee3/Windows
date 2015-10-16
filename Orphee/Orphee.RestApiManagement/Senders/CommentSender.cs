using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Orphee.RestApiManagement.Models;
using Orphee.RestApiManagement.Senders.Interfaces;

namespace Orphee.RestApiManagement.Senders
{
    /// <summary>
    /// Class sending comments to the
    /// remote server
    /// </summary>
    public class CommentSender : ICommentSender
    {
        private readonly INotificationSender _notifyer;

        /// <summary>
        /// Constructor initializing notifyer through 
        /// dependency injection
        /// </summary>
        /// <param name="notifyer">Instance of the NotificationSender class used to send notifications to the remote server</param>
        public CommentSender(INotificationSender notifyer)
        {
            this._notifyer = notifyer;
        }

        /// <summary>
        /// Sends a comment to the remote server
        /// </summary>
        /// <param name="comment">Comment to send</param>
        /// <param name="creationId">Target creation id</param>
        /// <returns>Returns true if the request was sent and the response received correctly. Returns false otherwise</returns>
        public async Task<bool> SendComment(string comment, string creationId)
        {
            var values = new Dictionary<string, string>()
            {
                {"creator", RestApiManagerBase.Instance.UserData.User.Id},
                {"creation", creationId},
                {"message", comment},
                {"parentId", creationId }
            };

            using (var httpClient = new HttpClient {BaseAddress = RestApiManagerBase.Instance.RestApiUrl})
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authorization", "Bearer " + RestApiManagerBase.Instance.UserData.Token);
                using (var content = new FormUrlEncodedContent(values))
                {
                    using (var response = await httpClient.PostAsync(RestApiManagerBase.Instance.RestApiPath["comment"], content))
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode)
                            return false;
                        var result = await this._notifyer.SendNotification("comments", creationId);
                        if (!result)
                            return false;
                    }
                }
                return true;
            }
        }
    }
}
