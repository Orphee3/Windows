using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.StartScreen;
using Orphee.RestApiManagement.Interfaces;

namespace Orphee.RestApiManagement
{
    public class CommentSender : ICommentSender
    {
        private readonly INotifyer _notifyer;
        public CommentSender(INotifyer notifyer)
        {
            this._notifyer = notifyer;
        }
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
                        string responseData = await response.Content.ReadAsStringAsync();
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
