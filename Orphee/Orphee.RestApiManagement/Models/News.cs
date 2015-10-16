using System;
using Orphee.RestApiManagement.Models.Interfaces;

namespace Orphee.RestApiManagement.Models
{
    /// <summary>
    /// Class representing the server
    /// News model representation
    /// </summary>
    public class News : INews
    {
        /// <summary>News id</summary>
        public string Id { get; set; }
        /// <summary>Creator of the news</summary>
        public User Creator { get; set; }
        /// <summary>Creation which the news comes from</summary>
        public Creation Creation { get; set; }
        /// <summary>Type of the news</summary>
        public string NewsType { get; set; }
        /// <summary>Creation date of the news</summary>
        public DateTime DateCreation { get; set; }
        /// <summary>True if the news was viewed. False otherwise</summary>
        public bool HasBeenViewed { get; set; }
    }
}
