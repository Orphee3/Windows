// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceResponse.cs" company="saramgsilva">
//   Copyright (c) 2013 saramgsilva. All rights reserved. 
// </copyright>
// <summary>
//   Defines the ServiceResponse type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Orphee.Models.OAuth2SDK.Services.Model
{
    public class ServiceResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string id_token { get; set; }
        public string refresh_token { get; set; }
    }
}
