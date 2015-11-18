// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISessionService.cs" company="saramgsilva">
//   Copyright (c) 2013 saramgsilva. All rights reserved. 
// </copyright>
// <summary>
//   Defines the ISessionService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Orphee.Models.OAuth2SDK.Services.Model;

namespace Orphee.Models.OAuth2SDK.Services.Interfaces
{
    /// <summary>
    /// The SessionService interface.
    /// </summary>
    public interface ISessionService
    {
        /// <summary>
        /// The get session.
        /// </summary>
        /// <returns>
        /// The <see cref="Session"/>.
        /// </returns>
        Task<Session> GetSession();
        
        /// <summary>
        /// The login async.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> object.
        /// </returns>
        Task<bool?> LoginAsync(string provider);

        /// <summary>
        /// The logout.
        /// </summary>
        void Logout();
    }
}
