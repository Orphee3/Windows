﻿using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Models;

namespace Orphee.RestApiManagement.Getters
{
    public class CreationCommentGetter : ICreationCommentGetter
    {
        public async Task<List<Comment>> GetCreationComments(string creationId)
        {
            List<Comment> comments = new List<Comment>();
            using (var httpClient = new HttpClient { BaseAddress = RestApiManagerBase.Instance.RestApiUrl })
            {
                using (var response = await httpClient.GetAsync(RestApiManagerBase.Instance.RestApiPath["comment"] + "/creation/" + creationId ))
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK || result == "")
                        return null;
                    comments = JsonConvert.DeserializeObject<List<Comment>>(result);
                }
            }
            return comments;
        }
    }
}
