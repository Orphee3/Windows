using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Orphee.RestApiManagement.Interfaces;

namespace Orphee.RestApiManagement
{
    public class User : IUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public JArray Friends { get; set; }
        public JArray Comments { get; set; }
        public JArray Likes { get; set; }
        public JArray Creations { get; set; }
        public Dictionary<string, ICreationUrls> CreationGetPutKeyList { get; set; }

        public User()
        {
            this.CreationGetPutKeyList = new Dictionary<string, ICreationUrls>();
        }
    }
}
