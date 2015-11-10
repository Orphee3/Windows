using System;
using System.Runtime.Serialization;
using Orphee.RestApiManagement.Models.Interfaces;

namespace Orphee.RestApiManagement.Models
{
    /// <summary>
    /// Class representing the server
    /// News model representation
    /// </summary>
    [DataContract]
    public class News : INews
    {
        /// <summary>News id</summary>
        [DataMember]
        public string Id { get; set; }
        /// <summary>Creator of the news</summary>
        [DataMember]
        public UserBase Creator { get; set; }
        /// <summary>Creation which the news comes from</summary>
        [DataMember]
        public Creation Creation { get; set; }
        /// <summary>Type of the news</summary>
        [DataMember]
        public string Type { get; set; }
        /// <summary>Creation date of the news</summary>
        [DataMember]
        public DateTime DateCreation { get; set; }
        /// <summary>True if the news was viewed. False otherwise</summary>
        [DataMember]
        public bool HasBeenViewed { get; set; }
        [DataMember]
        public string Message { get; set; }
    }
}
