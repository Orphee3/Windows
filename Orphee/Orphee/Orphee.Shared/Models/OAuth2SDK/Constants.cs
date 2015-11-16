// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="saramgsilva">
//   Copyright (c) 2013 saramgsilva. All rights reserved. 
// </copyright>
// <summary>
//   Defines the constants strings used in the app.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable once CheckNamespace
namespace Orphee.Models.OAuth2SDK
{
    /// <summary>
    /// Defines the constants strings used in the app.
    /// </summary>
    public class Constants
    {
        /// <summary> 
        /// The google callback url. 
        /// </summary> 
#if !WINDOWS_PHONE_APP
        public const string GoogleCallbackUrl = "urn:ietf:wg:oauth:2.0:oob";
#else
        public const string GoogleCallbackUrl = "http://localhost";
#endif

        /// <summary> 
        /// The facebook app id. 
        /// </summary> 
        public const string FacebookAppId = "560109060810772";

        /// <summary> 
        /// The google client identifier. 
        /// </summary> 
        public const string GoogleClientId = "1091784243585-rj2d5qa6eb6umdo8nlki2ao4h01rjjmc.apps.googleusercontent.com";

        /// <summary> 
        /// The google client secret. 
        /// </summary> 
        public const string GoogleClientSecret = "hzan1WDfhgswySp1xWVStmvj";

        /// <summary> 
        /// The login token. 
        /// </summary> 
        public const string LoginToken = "LoginToken";

        /// <summary> 
        /// The facebook provider. 
        /// </summary> 
        public const string FacebookProvider = "facebook";

        /// <summary> 
        /// The google provider. 
        /// </summary> 
        public const string GoogleProvider = "google";

        /// <summary> 
        /// The microsoft provider. 
        /// </summary> 
        public const string MicrosoftProvider = "microsoft"; 
    }
}
