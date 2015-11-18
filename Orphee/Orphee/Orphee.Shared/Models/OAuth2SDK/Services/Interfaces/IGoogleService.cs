// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGoogleService.cs" company="saramgsilva">
//   Copyright (c) 2013 saramgsilva. All rights reserved. 
// </copyright>
// <summary>
//   The GoogleService interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Orphee.Models.OAuth2SDK.Services.Model;

namespace Orphee.Models.OAuth2SDK.Services.Interfaces
{
    /// <summary>
    /// The GoogleService interface.
    /// </summary>
    public interface IGoogleService : ISessionProvider
    {
    }
}
