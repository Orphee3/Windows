// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FacebookService.cs" company="saramgsilva">
//   Copyright (c) 2013 saramgsilva. All rights reserved. 
// </copyright>
// <summary>
//   Defines the Facebook Service
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Data.Json;
using Windows.Security.Authentication.Web;
using Orphee.Models.OAuth2SDK.Services.Interfaces;
using Orphee.Models.OAuth2SDK.Services.Model;

// ReSharper disable once CheckNamespace
namespace Orphee.Models.OAuth2SDK.Services
{
    /// <summary>
    /// Defines the Facebook Service.
    /// </summary>
    public class FacebookService : IFacebookService
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookService"/> class.
        /// </summary>
        /// <param name="logManager">
        /// The log manager.
        /// </param>
        public FacebookService()
        {
        }

        /// <summary>
        /// The login sync.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/> object.
        /// </returns>
        public async Task<Session> LoginAsync()
        {
            const string FacebookCallbackUrl = "https://m.facebook.com/connect/login_success.html";
            var facebookUrl = "https://www.facebook.com/dialog/oauth?client_id=" + Uri.EscapeDataString(Constants.FacebookAppId) + "&redirect_uri=" + Uri.EscapeDataString(FacebookCallbackUrl) + "&scope=public_profile,email&display=popup&response_type=token";

            var startUri = new Uri(facebookUrl);
            var endUri = new Uri(FacebookCallbackUrl);

#if WINDOWS_PHONE_APP
            WebAuthenticationBroker.AuthenticateAndContinue(startUri, endUri, null, WebAuthenticationOptions.None);
            return null;
#else
            var webAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, startUri, endUri);
            return GetSession(webAuthenticationResult);
#endif
        }
        
        private void GetKeyValues(string webAuthResultResponseData, out string accessToken, out string expiresIn)
        {
            string responseData = webAuthResultResponseData.Substring(webAuthResultResponseData.IndexOf("access_token", StringComparison.Ordinal));
            string[] keyValPairs = responseData.Split('&');
            accessToken = null;
            expiresIn = null;
            for (int i = 0; i < keyValPairs.Length; i++)
            {
                string[] splits = keyValPairs[i].Split('=');
                switch (splits[0])
                {
                    case "access_token":
                        accessToken = splits[1];
                        break;
                    case "expires_in":
                        expiresIn = splits[1];
                        break;
                }
            }
        }

        /// <summary>
        /// This function extracts access_token from the response returned from web authentication broker
        /// and uses that token to get user information using facebook graph api. 
        /// </summary>
        /// <param name="accessToken">
        /// The access Token.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task<UserInfo> GetFacebookUserNameAsync(string accessToken)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(new Uri("https://graph.facebook.com/me?access_token=" + accessToken));
            var value = JsonValue.Parse(response).GetObject();
            var facebookUserName = value.GetNamedString("name");

            return new UserInfo
            {
                Name = facebookUserName,
            };
        }

        /// <summary>
        /// Logouts this instance.
        /// </summary>
        public async void Logout()
        {
            Exception exception = null;
            try
            {
               
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            if (exception != null)
            {
                return;
            }
        }

        private Session GetSession(WebAuthenticationResult result)
        {
            if (result.ResponseStatus == WebAuthenticationStatus.Success)
            {
                string accessToken;
                string expiresIn;
                GetKeyValues(result.ResponseData, out accessToken, out expiresIn);

                return new Session
                {
                    AccessToken = accessToken,
                    ExpireDate = new DateTime(long.Parse(expiresIn)),
                    Provider = Constants.FacebookProvider
                };
            }
            if (result.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
            {
                throw new Exception("Error http");
            }
            if (result.ResponseStatus == WebAuthenticationStatus.UserCancel)
            {
                throw new Exception("User Canceled.");
            }
            return null;
        }
    }
}
