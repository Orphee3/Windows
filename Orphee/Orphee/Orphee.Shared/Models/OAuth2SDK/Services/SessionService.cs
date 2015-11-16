// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SessionService.cs" company="saramgsilva">
//   Copyright (c) 2013 saramgsilva. All rights reserved. 
// </copyright>
// <summary>
//   Defines the ServiceSession type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Globalization;
using System.Threading.Tasks;
using Orphee.Models.OAuth2SDK.Cimbalino;
using Orphee.Models.OAuth2SDK.Services.Interfaces;
using Orphee.Models.OAuth2SDK.Services.Model;

// ReSharper disable once CheckNamespace
namespace Orphee.Models.OAuth2SDK.Services
{
    /// <summary>
    /// The service session.
    /// </summary>
    public class SessionService : ISessionService
    {
        private readonly IApplicationDataService _applicationSettings;
        private readonly IFacebookService _facebookService;
        private readonly IGoogleService _googleService;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SessionService" /> class.
        /// </summary>
        /// <param name="applicationSettings">The application settings.</param>
        /// <param name="facebookService">The facebook service.</param>
        /// <param name="microsoftService">The microsoft service.</param>
        /// <param name="googleService">The google service.</param>
        /// <param name="logManager">The log manager.</param>
        public SessionService(IApplicationDataService applicationSettings,
            IFacebookService facebookService,
            IGoogleService googleService)
        {
            _applicationSettings = applicationSettings;
            _facebookService = facebookService;
            _googleService = googleService;
        }

        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <returns>The session object.</returns>
        public async Task<Session> GetSession()
        {
            var expiryValue = DateTime.MinValue;
            string expiryTicks = LoadEncryptedSettingValue("session_expiredate");
            if (!string.IsNullOrWhiteSpace(expiryTicks))
            {
                long expiryTicksValue;
                if (long.TryParse(expiryTicks, out expiryTicksValue))
                {
                    expiryValue = new DateTime(expiryTicksValue);
                }
            }

            var result = new Session
            {
                Code = LoadEncryptedSettingValue("session_code"),
                AccessToken = LoadEncryptedSettingValue("session_token"),
                Id = LoadEncryptedSettingValue("session_id"),
                ExpireDate = expiryValue,
                Provider = LoadEncryptedSettingValue("session_provider")
            };

            var session = result;
            _applicationSettings.LocalSettings[Constants.LoginToken] = true;
            return session;
        }

        /// <summary>
        /// The save session.
        /// </summary>
        /// <param name="session">
        /// The session.
        /// </param>
        private void Save(Session session)
        {
            SaveEncryptedSettingValue("session_code", session.Code.Split('=')[1]);
            SaveEncryptedSettingValue("session_token", session.AccessToken);
            SaveEncryptedSettingValue("session_id", session.Id);
            SaveEncryptedSettingValue("session_expiredate", session.ExpireDate.Ticks.ToString(CultureInfo.InvariantCulture));
            SaveEncryptedSettingValue("session_provider", session.Provider);
            _applicationSettings.LocalSettings[Constants.LoginToken] = true;
        }

        /// <summary>
        /// The clean session.
        /// </summary>
        private void CleanSession()
        {
            _applicationSettings.LocalSettings.Remove("session_code");
            _applicationSettings.LocalSettings.Remove("session_token");
            _applicationSettings.LocalSettings.Remove("session_id");
            _applicationSettings.LocalSettings.Remove("session_expiredate");
            _applicationSettings.LocalSettings.Remove("session_provider");
            _applicationSettings.LocalSettings.Remove(Constants.LoginToken);
        }

        /// <summary>
        /// The login async.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> object.
        /// </returns>
        public async Task<bool?> LoginAsync(string provider)
        {
            Provider = provider;
            Exception exception;
            try
            {
                Session session = null;
                switch (provider)
                {
                    case Constants.FacebookProvider:
                        session = await _facebookService.LoginAsync();
                        break;
                    case Constants.GoogleProvider:
                        session = await _googleService.LoginAsync();
                        break;
                }
                if (session == null)
                {
                    return null;
                }
                Save(session);
                return true;
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            return false;
        }

        /// <summary>
        /// Gets or sets the provider.
        /// </summary>
        /// <value>
        /// The provider.
        /// </value>
        public string Provider { get; set; }

        /// <summary>
        /// The logout.
        /// </summary>
        public async void Logout()
        {
            Exception exception = null;
            try
            {
                var session = await GetSession();
                switch (session.Provider)
                {
                    case Constants.FacebookProvider:
                        _facebookService.Logout();
                        break;
                    case Constants.GoogleProvider:
                        _googleService.Logout();
                        break;
                }
                CleanSession();
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

#if WINDOWS_PHONE_APP
        public async Task<bool> Finalize(WebAuthenticationBrokerContinuationEventArgs args)
        {
            Exception exception = null;
            try
            {
                Session session = null;
                switch (Provider)
                {
                    case Constants.FacebookProvider:
                        session = await _facebookService.Finalize(args);
                        break;
                    case Constants.GoogleProvider:
                        session = await _googleService.Finalize(args);
                        break;
                }
                return true;
            }
            catch (Exception e)
            {
                exception = e;
            }
            return false;
        }
#endif
        /// <summary>
        /// Loads an encrypted setting value for a given key.
        /// </summary>
        /// <param name="key">
        /// The key to load.
        /// </param>
        /// <returns>
        /// The value of the key.
        /// </returns>
        private string LoadEncryptedSettingValue(string key)
        {
            string value = null;

            var protectedBytes = _applicationSettings.LocalSettings[key];
            if (protectedBytes != null)
            {
                // todo use DataProtectionProvider
                // byte[] valueBytes = ProtectedData.Unprotect(protectedBytes, null);
                // value = Encoding.UTF8.GetString(valueBytes, 0, valueBytes.Length);
                value = protectedBytes.ToString();
            }

            return value;
        }

        /// <summary>
        /// Saves a setting value against a given key, encrypted.
        /// </summary>
        /// <param name="key">
        /// The key to save against.
        /// </param>
        /// <param name="value">
        /// The value to save against.
        /// </param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The key or value provided is unexpected.
        /// </exception>
        private void SaveEncryptedSettingValue(string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
            {
                // todo use DataProtectionProvider
                // byte[] valueBytes = Encoding.UTF8.GetBytes(value);
                // var dataProtectionProvider = new DataProtectionProvider();          
                // // Encrypt the value by using the Protect() method.
                // byte[] protectedBytes = await dataProtectionProvider.ProtectAsync(valueBytes);
                _applicationSettings.LocalSettings[key] = value;
            }
        }
    }
}
