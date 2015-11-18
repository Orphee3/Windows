// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GoogleService.cs" company="saramgsilva">
//   Copyright (c) 2013 saramgsilva. All rights reserved. 
// </copyright>
// <summary>
//   The google service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Security.Authentication.Web;
using Windows.ApplicationModel.Activation;
using Orphee.Models.OAuth2SDK.Services.Interfaces;
using Orphee.Models.OAuth2SDK.Services.Model;

// ReSharper disable once CheckNamespace
namespace Orphee.Models.OAuth2SDK.Services
{
    /// <summary>
    /// The google service.
    /// </summary>
    public class GoogleService : IGoogleService
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleService"/> class.
        /// </summary>
        /// <param name="logManager">
        /// The log manager.
        /// </param>
        public GoogleService()
        {
        }

        /// <summary>
        /// The login async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/> object.
        /// </returns>
        public async Task<Session> LoginAsync()
        {
            var googleUrl = new StringBuilder();
            googleUrl.Append("https://accounts.google.com/o/oauth2/auth?client_id=");
            googleUrl.Append(Uri.EscapeDataString(Constants.GoogleClientId));
            googleUrl.Append("&scope=openid%20email%20profile");
            googleUrl.Append("&redirect_uri=");
            googleUrl.Append(Uri.EscapeDataString(Constants.GoogleCallbackUrl));
            googleUrl.Append("&state=foobar");
            googleUrl.Append("&response_type=code");

            var startUri = new Uri(googleUrl.ToString());


#if !WINDOWS_PHONE_APP
            var endUri = new Uri("https://accounts.google.com/o/oauth2/approval?");
            var webAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.UseTitle, startUri, endUri);
            return await GetSession(webAuthenticationResult);
#else
            WebAuthenticationBroker.AuthenticateAndContinue(startUri, new Uri(Constants.GoogleCallbackUrl), null, WebAuthenticationOptions.None);
            return null;
#endif
        }

        private string GetCode(string webAuthResultResponseData)
        {
            // Success code=4/izytpEU6PjuO5KKPNWSB4LK3FU1c
            var split = webAuthResultResponseData.Split('&');

            return split.FirstOrDefault(value => value.Contains("code"));
        }

        /// <summary>
        /// The logout.
        /// </summary>
        public void Logout()
        {
        }

        private async Task<Session> GetSession(WebAuthenticationResult result)
        {
            if (result.ResponseStatus == WebAuthenticationStatus.Success)
            {
                var code = GetCode(result.ResponseData);

                return new Session
                {
                    Code = code,
                    Provider = Constants.GoogleProvider
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
