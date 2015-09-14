using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Orphee.RestApiManagement.Interfaces;

namespace Orphee.RestApiManagement
{
    public class LikeSender : ILikeSender
    {
        private readonly INotifyer _notifyer;

        public LikeSender(INotifyer notifyer)
        {
            this._notifyer = notifyer;
        }
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
