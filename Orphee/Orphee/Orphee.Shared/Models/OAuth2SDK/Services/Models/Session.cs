// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Session.cs" company="saramgsilva">
//   Copyright (c) 2013 saramgsilva. All rights reserved. 
// </copyright>
// <summary>
//   Defines the Session type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Orphee.Models.OAuth2SDK.Services.Model
{
    /// <summary>
    /// The session.
    /// </summary>
    public class Session
    {
        public string Code { get; set; }
        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        /// <value>
        /// The access token.
        /// </value>
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id value.
        /// </value>
        public string Id { get; set; }
        
        /// <summary>
        /// Gets or sets the expire date.
        /// </summary>
        /// <value>
        /// The expire date.
        /// </value>
        public DateTime ExpireDate { get; set; }

        /// <summary>
        /// Gets or sets the provider.
        /// </summary>
        /// <value>
        /// The provider.
        /// </value>
        public string Provider { get; set; }
    }
}
